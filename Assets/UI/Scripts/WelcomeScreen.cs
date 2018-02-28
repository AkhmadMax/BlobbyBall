using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WelcomeScreen : MonoBehaviour
{
    static WelcomeScreen instance;

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
