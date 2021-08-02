using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualiserToggle : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    private float canvasTargetAlpha = 1;
    private Image buttonImage;
    [SerializeField] private Sprite[] buttonSprites;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }
    public void SetCanvasAlpha()
    {
        if(canvasTargetAlpha == 1)
        {
            canvasTargetAlpha = 0;
            buttonImage.sprite = buttonSprites[1];
        }
        else
        {
            canvasTargetAlpha = 1;
            buttonImage.sprite = buttonSprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, canvasTargetAlpha, Time.deltaTime);
    }
}
