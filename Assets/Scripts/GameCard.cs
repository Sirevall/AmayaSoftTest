using UnityEngine;
using UnityEngine.UI;

public class GameCard : MonoBehaviour
{
    public int cardType;

    private Card[] _levelCollection;
    private GameManager _gameManager;

    private Image _gameCardImage;
    private string _gameCardKey;

    private Animation _cardAnimation;
    private Animation _imageAnimation;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _cardAnimation = GetComponent<Animation>();
        _gameCardImage = transform.GetChild(0).GetComponent<Image>();
        _imageAnimation = transform.GetChild(0).GetComponent<Animation>();
        _particleSystem = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        _gameManager.createdCollection.AddListener(SetGameCardParameters);
        _gameManager.appear.AddListener(AppearanceWithBounceEffect);
    }
    private void OnDisable()
    {
        _gameManager.createdCollection.RemoveListener(SetGameCardParameters);
    }
    public string CardKey
    {
        get
        {
            return _gameCardKey;
        }
    }
    public void SetGameCardParameters()
    {
        _levelCollection = _gameManager.LevelCollection;
        _gameCardImage.sprite = _levelCollection[cardType].taskImage;
        _gameCardKey = _levelCollection[cardType].taskKey;
    }
    public void ChoseRightAnswer()
    {
        _imageAnimation.Play("BouncingFadeOut");
    }
    public void ChoseWrongAnswer()
    {
        _imageAnimation.Play("EaseInBounce");
    }
    public void AppearanceWithBounceEffect()
    {
        _cardAnimation.Play("BouncingAppearance");
    }
    public void PlayParticles()
    {
        _particleSystem.Play();
    }
}
