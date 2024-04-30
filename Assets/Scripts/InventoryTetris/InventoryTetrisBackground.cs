using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTetrisBackground : MonoBehaviour {

    [SerializeField] private InventoryTetris inventoryTetris;
    private Transform backgroundGrid;

    private void Start() {
        // Create background
        backgroundGrid = transform.Find("BackgroundGrid");
        Transform template = backgroundGrid.Find("Template");
        template.gameObject.SetActive(false);

        for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++) {
            for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++) {
                Transform backgroundSingleTransform = Instantiate(template, backgroundGrid);
                backgroundSingleTransform.gameObject.SetActive(true);
            }
        }

        backgroundGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(inventoryTetris.GetGrid().GetCellSize(), inventoryTetris.GetGrid().GetCellSize());

        backgroundGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(inventoryTetris.GetGrid().GetWidth(), inventoryTetris.GetGrid().GetHeight()) * inventoryTetris.GetGrid().GetCellSize();

        backgroundGrid.GetComponent<RectTransform>().anchoredPosition = inventoryTetris.GetComponent<RectTransform>().anchoredPosition;

    }

    public void ChangeBackgroundColor(List<Vector2Int> points)
    {
        RefreshBackground();
        foreach (Vector2Int point in points)
        {   
            Transform child = backgroundGrid.GetChild(point.x + point.y * inventoryTetris.GetGrid().GetWidth() + 1);
            child.GetComponent<Image>().color = new UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.25f);
        }

    }

    public void RefreshBackground()
    {
        for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++)
        {
            for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++)
            {
                Transform child = backgroundGrid.GetChild(x + y * inventoryTetris.GetGrid().GetWidth() + 1);
                child.GetComponent<Image>().color = new UnityEngine.Color (1f,1f,1f,0.25f);
            }
        }
    }
}