using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static CodeMonkey.Utils.UI_TextComplex;

public class InspectorUI : MonoBehaviour
{
    private InventoryBag inventoryBag;
    InventoryTetris inventoryTetris;
    private ItemTetrisPlacedObject.ItemAttribute itemAttribute;

    private Transform Str;
    private Transform Con;
    private Transform Int;
    private Transform Men;
    private Transform Agi;
    private Transform Rct;
    private Transform Cha;
    private Transform San;
    private Transform Price;
    private void Awake()
    {
        Str = transform.Find("Str");
        Con = transform.Find("Con");
        Int = transform.Find("Int");
        Men = transform.Find("Men");
        Agi = transform.Find("Agi");
        Rct = transform.Find("Rct");
        Cha = transform.Find("Cha");
        San = transform.Find("San");
        Price = transform.Find("Price");
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    private void RefreshInspectorUI()
    {
        itemAttribute = inventoryTetris.AttributeSum();
        TextMeshProUGUI uiText = Str.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText(""+itemAttribute.str);
         uiText = Con.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.con);
         uiText = Int.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.intl);
        uiText = Men.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.men);
        uiText = Agi.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.agi);
        uiText = Rct.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.rct);
        uiText = Cha.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.cha);
        uiText = San.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.san);
        uiText = Price.Find("Text").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + itemAttribute.price);
    }

    public void SetInventory(InventoryBag inventoryBag, InventoryTetris inventoryTetris)
    {
        this.inventoryBag = inventoryBag;
        this.inventoryTetris = inventoryTetris;

        inventoryTetris.OnObjectPlaced += InventoryTetris_OnObjectPlaced;
        inventoryBag.OnInventoryBagChange += InventoryBag_OnInventoryBagChange; ;
    }

    private void InventoryBag_OnInventoryBagChange(object sender, EventArgs e)
    {
        inventoryTetris.RefreshAllAttribute();
        RefreshInspectorUI();
    }

    private void InventoryTetris_OnObjectPlaced(object sender, ItemTetrisPlacedObject e)
    {
        inventoryTetris.RefreshAllAttribute();
        RefreshInspectorUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
