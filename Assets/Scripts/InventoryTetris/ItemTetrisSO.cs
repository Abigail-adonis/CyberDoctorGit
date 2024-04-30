using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class ItemTetrisSO : ScriptableObject
{

    public string nameString;
    public string descriptionString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;
    public List<Vector2Int> points;

    public int Str;//力量
    public int Con;//体质
    public int Agi;//敏捷
    public int Int;//智力
    public int Men;//精神
    public int Cha;//魅力
    public int Rct;//反应
    public int San;//耐受
    public int Price;//价格
    public int amount = 1;

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }

    public static Vector2Int GetDirForwardVector(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, -1);
            case Dir.Left: return new Vector2Int(-1, 0);
            case Dir.Up: return new Vector2Int(0, +1);
            case Dir.Right: return new Vector2Int(+1, 0);
        }
    }

    public static Dir GetDir(Vector2Int from, Vector2Int to)
    {
        if (from.x < to.x)
        {
            return Dir.Right;
        }
        else
        {
            if (from.x > to.x)
            {
                return Dir.Left;
            }
            else
            {
                if (from.y < to.y)
                {
                    return Dir.Up;
                }
                else
                {
                    return Dir.Down;
                }
            }
        }
    }

    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }




    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, 1);
            case Dir.Up: return new Vector2Int(1, 1);
            case Dir.Right: return new Vector2Int(1, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
                points.ForEach(point => gridPositionList.Add(offset + new Vector2Int(point.x, point.y)));
                break;
            case Dir.Up:
                points.ForEach(point => gridPositionList.Add(offset + new Vector2Int(-point.x, -point.y)));
                break;
            case Dir.Left:
                points.ForEach(point => gridPositionList.Add(offset + new Vector2Int(point.y, -point.x)));
                break;
            case Dir.Right:
                points.ForEach(point => gridPositionList.Add(offset + new Vector2Int(-point.y, point.x)));
                break;
        }
        return gridPositionList;
    }


    public static Transform CreateVisualGrid(Transform visualParentTransform, ItemTetrisSO itemTetrisSO, float cellSize) {
        Transform visualTransform = Instantiate(InventoryTetrisAssets.Instance.gridVisual, visualParentTransform);

        // Create background
        Transform template = visualTransform.Find("Template");
        template.gameObject.SetActive(false);
        itemTetrisSO.points.ForEach(point  =>
        {
            Transform backgroundSingleTransform = Instantiate(template, visualTransform);
            backgroundSingleTransform.gameObject.SetActive(true);
            backgroundSingleTransform.GetComponent<RectTransform>().localPosition = new Vector3(point.x * cellSize, point.y * cellSize);
        });
        //for (int x = 0; x < itemTetrisSO.width; x++) {
        //    for (int y = 0; y < itemTetrisSO.height; y++) {
        //    }
        //}

        //visualTransform.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * cellSize;

        //visualTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(itemTetrisSO.width, itemTetrisSO.height) * cellSize;

        visualTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f * cellSize, 0.5f* cellSize);
        visualTransform.SetAsFirstSibling();

        return visualTransform;
    }

    public Sprite GetSprite()
    {
        switch (nameString)
        {
            default:
            case "ModuleA": return InventoryTetrisAssets.Instance.ModuleASprite;
            case "ModuleB1": return InventoryTetrisAssets.Instance.ModuleB1Sprite;
            case "ModuleB2": return InventoryTetrisAssets.Instance.ModuleB2Sprite;
            case "ModuleC": return InventoryTetrisAssets.Instance.ModuleCSprite;
            case "ModuleD1": return InventoryTetrisAssets.Instance.ModuleD1Sprite;
            case "ModuleD2": return InventoryTetrisAssets.Instance.ModuleD2Sprite;
            case "ModuleE": return InventoryTetrisAssets.Instance.ModuleESprite;
            case "ModuleF1": return InventoryTetrisAssets.Instance.ModuleF1Sprite;
            case "ModuleF2": return InventoryTetrisAssets.Instance.ModuleF2Sprite;
            case "Heart1": return InventoryTetrisAssets.Instance.Heart1Sprite;
            case "Heart2": return InventoryTetrisAssets.Instance.Heart2Sprite;
            case "Heart3": return InventoryTetrisAssets.Instance.Heart3Sprite;
            case "Heart4": return InventoryTetrisAssets.Instance.Heart4Sprite;
            case "Heart5": return InventoryTetrisAssets.Instance.Heart5Sprite;
            case "Heart6": return InventoryTetrisAssets.Instance.Heart6Sprite;
            case "Heart7": return InventoryTetrisAssets.Instance.Heart7Sprite;
            case "ModuleG": return InventoryTetrisAssets.Instance.ModuleGSprite;
            case "ModuleH": return InventoryTetrisAssets.Instance.ModuleHSprite;
            case "ModuleI1": return InventoryTetrisAssets.Instance.ModuleI1Sprite;
            case "ModuleI2": return InventoryTetrisAssets.Instance.ModuleI2Sprite;
            case "ModuleI3": return InventoryTetrisAssets.Instance.ModuleI3Sprite;
            case "ModuleI4": return InventoryTetrisAssets.Instance.ModuleI4Sprite;
        }
    }
}
