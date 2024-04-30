/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;



public class Tooltip_ItemStats : MonoBehaviour
{

    private static Tooltip_ItemStats instance;

    [SerializeField]
    private RectTransform canvasRectTransform = null;

    private Image image;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        image = transform.Find("image").GetComponent<Image>();
        nameText = transform.Find("nameText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("descriptionText").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }

    private void ShowTooltip(Sprite itemSprite, string itemName, string itemDescription)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        nameText.text = itemName;
        descriptionText.text = itemDescription;
        image.sprite = itemSprite;
        Update();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(Sprite itemSprite, string itemName, string itemDescription)
    {
        instance.ShowTooltip(itemSprite, itemName, itemDescription);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }





    public static void AddTooltip(Transform transform, Sprite itemSprite, string itemName, string itemDescription)
    {
        if (transform.GetComponent<Button_UI>() != null)
        {
            transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => Tooltip_ItemStats.ShowTooltip_Static(itemSprite, itemName, itemDescription);
            transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => Tooltip_ItemStats.HideTooltip_Static();
        }
    }

}

