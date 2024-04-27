using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static InventoryBag;

public class InventoryBagUI : MonoBehaviour
{
    enum SortType
    {
        Str = 0,//力量
        Con,//体质
        Agi,//敏捷
        Int,//智力
        Men,//精神
        Cha,//魅力
        Rct,//反应
        San,//耐受
        Price,//价格
        Amount,
        PointNum,
    };

    private InventoryBag inventoryBag;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Transform itemViewpoint;
    private List<Item> itemList;
    private SortType sortType = SortType.PointNum;
    private int sortOrder = 1;

    private void Awake()
    {
        itemViewpoint = transform.Find("itemViewpoint");
        itemSlotContainer = itemViewpoint.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        InventoryTetrisManualPlacement.Instance.OnObjectPlaced += Instance_OnObjectPlaced; 
    }

    private void Instance_OnObjectPlaced(object sender, EventArgs e)
    {
        if (inventoryBag.RemoveItemTetrisSO(InventoryTetrisManualPlacement.Instance.GetItemTetrisSO(), 1))
        {
            InventoryTetrisManualPlacement.Instance.DeselectObjectType();
        }
    }

    public void SetInventory(InventoryBag inventoryBag)
    {
        this.inventoryBag = inventoryBag;
        itemList = inventoryBag.GetItemTetrisList();
        inventoryBag.OnInventoryBagChange += InventoryBag_OnInventoryBagChanged;
        RefreshInventoryBag();
    }

    private void InventoryBag_OnInventoryBagChanged(object sender, EventArgs e)
    {
        RefreshInventoryBag();
    }

    private void ItemFilter()
    {
        itemList = inventoryBag.GetItemTetrisList();
    }

    private void ItemSort()
    {
        itemList = inventoryBag.GetItemTetrisList();
        itemList.Sort(ItemTerisSort);
    }

    private int ItemTerisSort(Item A, Item B)
    {
        switch (sortType)
        {
            case SortType.Str:
                if (A.Str > B.Str)
                    return -sortOrder;
                else if (A.Str < B.Str)
                    return sortOrder;
                else
                    return 0;
            case SortType.Int:
                if (A.Int > B.Int)
                    return -sortOrder;
                else if (A.Int < B.Int)
                    return sortOrder;
                else
                    return 0;
            case SortType.Agi:
                if (A.Agi > B.Agi)
                    return -sortOrder;
                else if (A.Agi < B.Agi)
                    return sortOrder;
                else
                    return 0;
            case SortType.Con:
                if (A.Con > B.Con)
                    return -sortOrder;
                else if ((A.Con < B.Con))
                    return sortOrder;
                else return 0;
            case SortType.Men:
                if (A.Men > B.Men)
                    return -sortOrder;
                else if (A.Men < B.Men)
                    return sortOrder;
                else
                    return 0;
            case SortType.San:
                if (A.San > B.San)
                    return sortOrder;
                else if (A.San < B.San)
                    return -sortOrder;
                else return 0;
            case SortType.Rct:
                if (A.Rct > B.Rct)
                    return -sortOrder;
                else if (A.Rct < B.Rct)
                    return sortOrder;
                else return 0;
            case SortType.Cha:
                if (A.Cha > B.Cha)
                    return -sortOrder;
                else if (A.Cha < B.Cha)
                    return sortOrder;
                else return 0;
            case SortType.PointNum:
                if (A.pointCount > B.pointCount)
                    return sortOrder;
                else if (A.pointCount < B.pointCount)
                    return -sortOrder;
                else return 0;
            case SortType.Price:
                if (A.Price > B.Price)
                    return -sortOrder;
                else if ((A.Price < B.Price))
                    return sortOrder;
                else return 0;
        }
        return 1;
    }

    private void RefreshInventoryBag()
    {
        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        ItemSort();
        foreach (Item item in itemList)
        {
            if(item.amount > 0)
            {
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                        InventoryTetrisManualPlacement.Instance.ChangeSelectObjectType(item.nameString);
                };
                itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => {
                    Tooltip_ItemStats.ShowTooltip_Static(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(item.nameString).GetSprite(), item.nameString, item.descriptionString);
                };
                itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => {
                    Tooltip_ItemStats.HideTooltip_Static();
                };
                Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
                image.sprite = InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(item.nameString).GetSprite();
                TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
                uiText.SetText(item.amount.ToString());
            }

        }
    }

    public void MoveRight()
    {
        float containerWidth = itemSlotContainer.GetComponent<HorizontalLayoutGroup>().preferredWidth;
        float viewWidth = itemViewpoint.GetComponent<RectTransform>().rect.width;
        if(containerWidth/ viewWidth < 2f)
        {
            transform.GetComponent<ScrollRect>().horizontalNormalizedPosition += 1f;
        }
        else
        {
            transform.GetComponent<ScrollRect>().horizontalNormalizedPosition += viewWidth/(containerWidth - viewWidth);
        }

    }

    public void MoveLeft()
    {
        float containerWidth = itemSlotContainer.GetComponent<HorizontalLayoutGroup>().preferredWidth;
        float viewWidth = itemViewpoint.GetComponent<RectTransform>().rect.width;
        if (containerWidth / viewWidth < 2f)
        {
            transform.GetComponent<ScrollRect>().horizontalNormalizedPosition -= 1f;
        }
        else
        {
            transform.GetComponent<ScrollRect>().horizontalNormalizedPosition -= viewWidth / (containerWidth - viewWidth);
        }
    }

    public void SetSortTypeAttribute()
    {
        if (sortType >= SortType.Rct)
            sortType = SortType.Str;
        else if (sortType < SortType.Rct)
            sortType++;
        RefreshInventoryBag();
    }

    public void SetSortTypeSan()
    {
        sortType = SortType.San;

        RefreshInventoryBag();
    }

    public void SetSortTypePrice()
    {
        sortType = SortType.Price;

        RefreshInventoryBag();
    }

    public void SetSortTypePointNum()
    {
        sortType = SortType.PointNum;

        RefreshInventoryBag();
    }
}
