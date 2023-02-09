using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
///     JON23:  1. WelcomeScreen sounds like there are other screens. They all pobably can be at least Shown or Hidden. So introducing a Screen abstraction is not a bad idea
///             2. Using a Singleton pattern is not best approach here, having a UIManager is better to manage the UI Panels. UI Manager can be a Singleton if needed
/// </summary>
public class WelcomeScreen : MonoBehaviour
{
    static WelcomeScreen instance;

    // JON23:   A typo in Instance word
    public static WelcomeScreen Instane
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<WelcomeScreen>();

            return instance;
        }
    }

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0, 0.3f);
    }

    public void Show()
    {
        canvasGroup.DOFade(1, 0.3f);
        canvasGroup.interactable = true;
    }
}
