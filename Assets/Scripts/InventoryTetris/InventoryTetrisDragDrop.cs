﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTetrisDragDrop : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private InventoryTetris inventoryTetris;
    private ItemTetrisPlacedObject placedObject;

    private void Awake() {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        placedObject = GetComponent<ItemTetrisPlacedObject>();
    }

    public void Setup(InventoryTetris inventoryTetris) {
        this.inventoryTetris = inventoryTetris;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;

        //ItemTetrisSO.CreateVisualGrid(transform.GetChild(0), placedObject.GetItemTetrisSO(), inventoryTetris.GetGrid().GetCellSize());
        InventoryTetrisDragDropSystem.Instance.StartedDragging(inventoryTetris, placedObject, 
            ItemTetrisSO.CreateVisualGrid(transform.GetChild(0), placedObject.GetItemTetrisSO(), inventoryTetris.GetGrid().GetCellSize()));
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        //Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        InventoryTetrisDragDropSystem.Instance.StoppedDragging(inventoryTetris, placedObject);
    }

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("OnPointerDown");
        if(PointerEventData.InputButton.Left == eventData.button)
        {
            InventoryTetrisDragDropSystem.Instance.PointerClickLeft(inventoryTetris, placedObject);
        }
        else if(PointerEventData.InputButton.Right == eventData.button)
        {
            InventoryTetrisDragDropSystem.Instance.PointerClickRight(inventoryTetris, placedObject);
        }

    }

}