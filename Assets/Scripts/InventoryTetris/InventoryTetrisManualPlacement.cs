using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class InventoryTetrisManualPlacement : MonoBehaviour {

    public static InventoryTetrisManualPlacement Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField] private Canvas canvas = null;

    private List<ItemTetrisSO> itemTetrisSOList = null;
    private ItemTetrisSO itemTetrisSO;
    private ItemTetrisSO.Dir dir;
    private InventoryTetris inventoryTetris;
    private RectTransform canvasRectTransform;
    private RectTransform itemContainer;



    private void Awake() {
        Instance = this;

        inventoryTetris = GetComponent<InventoryTetris>();

        itemTetrisSO = null;

        if (canvas == null) {
            canvas = GetComponentInParent<Canvas>();
        }

        if (canvas != null) {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        itemTetrisSOList = InventoryTetrisAssets.Instance.itemTetrisSOList;
    }

    private void Update() {
        // Try to place
        if (Input.GetMouseButtonDown(0) && itemTetrisSO != null) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, Input.mousePosition, null, out Vector2 anchoredPosition);
            
            Vector2Int placedObjectOrigin = inventoryTetris.GetGridPosition(anchoredPosition);

            bool tryPlaceItem = inventoryTetris.TryPlaceItem(itemTetrisSO as ItemTetrisSO, placedObjectOrigin, dir);

            if (tryPlaceItem) {
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            } else {
                // Cannot build here
                TooltipCanvas.ShowTooltip_Static("Cannot Build Here!");
                FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)&& inventoryTetris.GetRotationState()) {
            dir = ItemTetrisSO.GetNextDir(dir);
        }

        if(Input.GetMouseButtonDown(1) && itemTetrisSO != null)
        {
            DeselectObjectType();
        }
    }

    public void DeselectObjectType() {
        itemTetrisSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeSelectObjectType(string stringName)
    {
        foreach(ItemTetrisSO targetType in itemTetrisSOList)
        {
            if(targetType.nameString == stringName)
            {
                itemTetrisSO = targetType;
                RefreshSelectedObjectType();
                break;
            }
        };
    }

    public Vector2 GetCanvasSnappedPosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, Input.mousePosition, null, out Vector2 anchoredPosition);
        inventoryTetris.GetGrid().GetXY(anchoredPosition, out int x, out int y);

        if (itemTetrisSO != null) {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector2 placedObjectCanvas = inventoryTetris.GetGrid().GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * inventoryTetris.GetGrid().GetCellSize();
            return placedObjectCanvas;
        } else {
            return anchoredPosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (itemTetrisSO != null) {
            return Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));
        } else {
            return Quaternion.identity;
        }
    }

    public ItemTetrisSO GetItemTetrisSO() {
        return itemTetrisSO;
    }



}
