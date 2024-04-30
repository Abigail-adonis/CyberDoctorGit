using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class InventoryTetrisDragDropSystem : MonoBehaviour {

    public static InventoryTetrisDragDropSystem Instance { get; private set; }



    [SerializeField] private List<InventoryTetris> inventoryTetrisList;

    private InventoryTetris draggingInventoryTetris;
    private ItemTetrisPlacedObject draggingPlacedObject;
    private Vector2Int mouseDragGridPositionOffset;
    private Vector2 mouseDragAnchoredPositionOffset;
    private ItemTetrisSO.Dir dir;
    private Transform gridVisual;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        foreach (InventoryTetris inventoryTetris in inventoryTetrisList) {
            inventoryTetris.OnObjectPlaced += (object sender, ItemTetrisPlacedObject placedObject) => {

            };
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)&& inventoryTetrisList[0].GetRotationState()) {
            dir = ItemTetrisSO.GetNextDir(dir);
        }

        if (draggingPlacedObject != null) {
            // Calculate target position to move the dragged item
            RectTransformUtility.ScreenPointToLocalPointInRectangle(draggingInventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 targetPosition);
            targetPosition += new Vector2(-mouseDragAnchoredPositionOffset.x, -mouseDragAnchoredPositionOffset.y);

            // Apply rotation offset to target position
            Vector2Int rotationOffset = draggingPlacedObject.GetItemTetrisSO().GetRotationOffset(dir);
            targetPosition += new Vector2(rotationOffset.x, rotationOffset.y) * draggingInventoryTetris.GetGrid().GetCellSize();

            // Snap position
            targetPosition /= 10f;// draggingInventoryTetris.GetGrid().GetCellSize();
            targetPosition = new Vector2(Mathf.Floor(targetPosition.x), Mathf.Floor(targetPosition.y));
            targetPosition *= 10f;

            // Move and rotate dragged object
            draggingPlacedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(draggingPlacedObject.GetComponent<RectTransform>().anchoredPosition, targetPosition, Time.deltaTime * 20f);
            draggingPlacedObject.transform.rotation = Quaternion.Lerp(draggingPlacedObject.transform.rotation, Quaternion.Euler(0, 0, -draggingPlacedObject.GetItemTetrisSO().GetRotationAngle(dir)), Time.deltaTime * 15f);
        }
    }
    private void LateUpdate()
    {
        if (draggingPlacedObject != null)
        {
            Vector3 screenPoint = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(draggingInventoryTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPosition);
            Vector2 anchored = draggingInventoryTetris.GetItemContainer().position;
            Vector2Int placedObjectOrigin = draggingInventoryTetris.GetGridPosition(anchoredPosition);
            placedObjectOrigin = placedObjectOrigin - mouseDragGridPositionOffset;
            Vector2 targetPosition = anchored + new Vector2( (placedObjectOrigin.x + 0.5f) * draggingInventoryTetris.GetGrid().GetCellSize(), (placedObjectOrigin.y + 0.5f) * draggingInventoryTetris.GetGrid().GetCellSize());
            gridVisual.position = Vector2.Lerp(gridVisual.position, targetPosition, Time.deltaTime * 15f);
        }

    }

    public void StartedDragging(InventoryTetris inventoryTetris, ItemTetrisPlacedObject placedObject, Transform visualTransfom) {
        // Started Dragging
        draggingInventoryTetris = inventoryTetris;
        draggingPlacedObject = placedObject;
        gridVisual = visualTransfom;
        Cursor.visible = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
        Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);

        // Calculate Grid Position offset from the placedObject origin to the mouseGridPosition
        mouseDragGridPositionOffset = mouseGridPosition - placedObject.GetGridPosition();

        // Calculate the anchored poisiton offset, where exactly on the image the player clicked
        mouseDragAnchoredPositionOffset = anchoredPosition - placedObject.GetComponent<RectTransform>().anchoredPosition;

        // Save initial direction when started draggign
        dir = placedObject.GetDir();

        // Apply rotation offset to drag anchored position offset
        Vector2Int rotationOffset = draggingPlacedObject.GetItemTetrisSO().GetRotationOffset(dir);
        mouseDragAnchoredPositionOffset += new Vector2(rotationOffset.x, rotationOffset.y) * draggingInventoryTetris.GetGrid().GetCellSize();
    }

    public void StoppedDragging(InventoryTetris fromInventoryTetris, ItemTetrisPlacedObject placedObject) {
        draggingInventoryTetris = null;
        draggingPlacedObject = null;

        Cursor.visible = true;

        // Remove item from its current inventory
        fromInventoryTetris.RemoveItemAt(placedObject);

        InventoryTetris toInventoryTetris = null;

        // Find out which InventoryTetris is under the mouse position
        foreach (InventoryTetris inventoryTetris in inventoryTetrisList) {
            Vector3 screenPoint = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPosition);
            Vector2Int placedObjectOrigin = inventoryTetris.GetGridPosition(anchoredPosition);
            placedObjectOrigin = placedObjectOrigin - mouseDragGridPositionOffset;

            if (inventoryTetris.IsValidGridPosition(placedObjectOrigin)) {
                toInventoryTetris = inventoryTetris;
                break;
            }
        }

        // Check if it's on top of a InventoryTetris
        if (toInventoryTetris != null) {
            Vector3 screenPoint = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInventoryTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPosition);
            Vector2Int placedObjectOrigin = toInventoryTetris.GetGridPosition(anchoredPosition);
            placedObjectOrigin = placedObjectOrigin - mouseDragGridPositionOffset;

            bool tryPlaceItem = toInventoryTetris.TryPlaceItem(placedObject.GetItemTetrisSO()  , placedObjectOrigin, dir);

            if (tryPlaceItem) {
                // Item placed!
            } else {
                // Cannot drop item here!
                TooltipCanvas.ShowTooltip_Static("Cannot Drop Item Here!");
                FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);

                // Drop on original position
                fromInventoryTetris.TryPlaceItem(placedObject.GetItemTetrisSO()  , placedObject.GetGridPosition(), placedObject.GetDir());
            }
        } else {
            // Not on top of any Inventory Tetris!

            // Cannot drop item here!
            TooltipCanvas.ShowTooltip_Static("Cannot Drop Item Here!");
            FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);

            // Drop on original position
            fromInventoryTetris.TryPlaceItem(placedObject.GetItemTetrisSO()  , placedObject.GetGridPosition(), placedObject.GetDir());
        }
    }
    public void PointerClickLeft(InventoryTetris fromInventoryTetris, ItemTetrisPlacedObject placedObject)
    {
        if (fromInventoryTetris.GetRotationState())
        {
            Cursor.visible = true;
            // Remove item from its current inventory
            fromInventoryTetris.RemoveItemAt(placedObject);

            bool tryPlaceItem = fromInventoryTetris.TryPlaceItem(placedObject.GetItemTetrisSO() , placedObject.GetGridPosition(), ItemTetrisSO.GetNextDir(dir));

            if (tryPlaceItem)
            {
                dir = ItemTetrisSO.GetNextDir(dir);
                // Item placed!
            }
            else
            {
                // Cannot drop item here!
                TooltipCanvas.ShowTooltip_Static("Cannot rotate here!");
                FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                // Drop on original position
                fromInventoryTetris.TryPlaceItem(placedObject.GetItemTetrisSO()  , placedObject.GetGridPosition(), placedObject.GetDir());
            }
        }

    }

    public void PointerClickRight(InventoryTetris fromInventoryTetris, ItemTetrisPlacedObject placedObject)
    {
        Cursor.visible = true;
        // Remove item from its current inventory
        fromInventoryTetris.MoveItemToBag(placedObject);
    }



}