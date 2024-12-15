using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Vector3 initialPosition;
    Vector3 awayPosition = new Vector3(-100,-100,-100);

    [Header("Fade")]
    [SerializeField] float time;
    bool fadingOut = false;
    bool fadingIn = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (fadingOut)
        {
            canvasGroup.alpha -= Time.deltaTime / (time + 0.001f);
            if(canvasGroup.alpha <= 0)
            {
                fadingOut = false;
                transform.position = awayPosition;
            }
        }

        if (fadingIn)
        {
            canvasGroup.alpha += Time.deltaTime / (time + 0.001f);
            if (canvasGroup.alpha >= 1)
            {
                fadingIn = false;
            }
        }
    }

    public void FadeOut()
    {
        fadingOut = true;
        canvasGroup.interactable = false;
    }

    public void FadeIn()
    {
        transform.position = initialPosition;
        fadingIn = true;
        canvasGroup.interactable = true;
    }


}
