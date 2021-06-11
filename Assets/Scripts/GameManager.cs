using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text _task;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameCard[] _gameCards;
    [SerializeField] private CollectionCards[] _collections;

    public UnityEvent createdCollection;
    public UnityEvent gameEnd;
    public UnityEvent restartGame;
    public UnityEvent appear;

    private Card[] _levelCollection;
    private int[] _numberGameCardsPerLevel = { 3, 6, 9 };
    private List<Card> _usedCards = new List<Card>();

    private string _levelTask;
    private int _level = 0;
    private int _numberElementCollections;
    private int _randomNumber;

    private Animation _taskAnimation;

    void Start()
    {
        _taskAnimation = _task.GetComponent<Animation>();
        CalculateElementCollections();
        SetCardType();
        EnableNextLevel(effects: true);
    }
    public Card[] LevelCollection
    {
        get
        {
            return _levelCollection;
        }
    }
    public void CheckAnswer(GameCard card)
    {
        if (_levelTask == card.CardKey)
        {
            if (_level < _numberGameCardsPerLevel.Length)
                StartCoroutine(ChoseRightAnswerCoroutine(card));
            else
                StartCoroutine(GameEndCoroutine(card));
        }
        else
        {
            card.ChoseWrongAnswer();
        }
    }
    public void RestartGameOnClick()
    {
        restartGame.Invoke();
        _restartButton.SetActive(false);
    }
    public void RestartGame()
    {
        foreach (var card in _gameCards)
        {
            card.gameObject.SetActive(false);
        }
        _level = 0;
        EnableNextLevel(effects: true);
    }

    private void EnableNextLevel(bool effects = false)
    {
        int numberGameCards = _numberGameCardsPerLevel[_level];

        SelectLevelCollection();
        ChooseLevelTask();
        EnableGameCards(numberGameCards);
        if (effects)
        {
            _taskAnimation.Play("FadeInAllText");
            appear.Invoke();
        }

        createdCollection.Invoke();

        DisplayTask();
        _level++;
    }
    private void SelectLevelCollection()
    {
        _levelCollection = _collections[Random.Range(0, _collections.Length)].cards;
    }
    private void SetCardType()
    {
        for (int i = 0; i < _gameCards.Length; i++)
        {
            _gameCards[i].cardType = i;
        }
    }
    private void ShuffleCollection()
    {
        for (int i = _levelCollection.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _levelCollection[j];
            _levelCollection[j] = _levelCollection[i];
            _levelCollection[i] = temp;
        }
    }
    private void EnableGameCards(int numberCards)
    {
        for (int i = 0; i < numberCards; i++)
        {
            _gameCards[i].gameObject.SetActive(true);
        }
    }
    private void ChooseLevelTask()
    {
        FindNewTask();
        _usedCards.Add(_levelCollection[_randomNumber]);
    }
    private void FindNewTask()
    {
        bool usedCard = false;
        CheckListIsFull();
        do
        {
            ShuffleCollection();
            _randomNumber = Random.Range(0, _numberGameCardsPerLevel[_level]);
            _levelTask = _levelCollection[_randomNumber].taskKey;
            usedCard = _usedCards.Exists(e => e.taskKey == _levelTask);
        } while (usedCard);
    }
    private void CheckListIsFull()
    {
        if (_usedCards.Count == _numberElementCollections)
            _usedCards.Clear();
    }
    private void PlayCardEffects(GameCard card)
    {
        card.ChoseRightAnswer();
        card.PlayParticles();
    }
    private void DisplayTask()
    {
        _task.text = $"Find {_levelTask}";
    }
    private void CalculateElementCollections()
    {
        foreach (var collection in _collections)
        {
            _numberElementCollections += collection.cards.Length;
        }
    }
    private IEnumerator ChoseRightAnswerCoroutine(GameCard card)
    {
        PlayCardEffects(card);
        yield return new WaitForSeconds(2.1f);
        EnableNextLevel();
    }
    private IEnumerator GameEndCoroutine(GameCard card)
    {
        PlayCardEffects(card);
        yield return new WaitForSeconds(2.1f);
        gameEnd.Invoke();
        _restartButton.SetActive(true);
    }
}
