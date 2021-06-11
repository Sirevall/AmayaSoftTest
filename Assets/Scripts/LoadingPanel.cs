using UnityEngine;
using UnityEngine.Events;

public class LoadingPanel : MonoBehaviour, IFadeController
{
    private Animation _animation;

    public UnityEvent loadPanelOff;

    private void OnEnable()
    {
        _animation = GetComponent<Animation>();
    }
    public void FadeIn(string animationName)
    {
        switch (animationName)
        {
            case "FadeInAllImage":
                _animation.Play("FadeInAllImage");
                break;
            case "FadeInHalfImage":
                _animation.Play("FadeInHalfImage");
                break;
        }
    }
    public void EnablePanel()
    {
        gameObject.SetActive(true);
        FadeIn("FadeInHalfImage");
    }
    public void DisablePanel()
    {
        loadPanelOff.Invoke();
        gameObject.SetActive(false);
    }
}
