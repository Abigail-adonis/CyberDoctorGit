using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTetrisPlacedObject : MonoBehaviour
{
    public static ItemTetrisPlacedObject CreateCanvas(Transform parent, Vector2 anchoredPosition, Vector2Int origin, ItemTetrisSO.Dir dir, ItemTetrisSO itemTetrisSO)
    {
        Transform placedObjectTransform = Instantiate(itemTetrisSO.prefab, parent);
        placedObjectTransform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));
        placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        ItemTetrisPlacedObject placedObject = placedObjectTransform.GetComponent<ItemTetrisPlacedObject>();
        placedObject.itemTetrisSO = itemTetrisSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.itemAttribute = new ItemAttribute
        {   str = itemTetrisSO.Str,
            con = itemTetrisSO.Con,
            intl = itemTetrisSO.Int,
            men = itemTetrisSO.Men,
            agi = itemTetrisSO.Agi,
            rct = itemTetrisSO.Rct,
            cha = itemTetrisSO.Cha,
            san = itemTetrisSO.San,
            price = itemTetrisSO.Price
        };
        return placedObject;
    }
    private ItemTetrisSO itemTetrisSO;
    private Vector2Int origin;
    private ItemTetrisSO.Dir dir;
    [Serializable]
    public struct ItemAttribute
    {
        public int str;
        public int con;
        public int intl;
        public int men;
        public int agi;
        public int rct;
        public int cha;
        public int san;
        public int price;
    }
    private ItemAttribute itemAttribute;
    // Start is called before the first frame update
    //void Start()
    //{
    //    InventoryTetris.Instance.OnObjectPlaced += Instance_OnObjectPlaced;
    //}

    //private void Instance_OnObjectPlaced(object sender, ItemTetrisPlacedObject e)
    //{

    //}

    public void RefreshAttribute()
    {
        itemAttribute = new ItemAttribute
        {
            str = itemTetrisSO.Str,
            con = itemTetrisSO.Con,
            intl = itemTetrisSO.Int,
            men = itemTetrisSO.Men,
            agi = itemTetrisSO.Agi,
            rct = itemTetrisSO.Rct,
            cha = itemTetrisSO.Cha,
            san = itemTetrisSO.San,
            price = itemTetrisSO.Price
        };
        switch (itemTetrisSO.nameString)
        {
            case "ModuleA":
                break;
            case "ModuleB1":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleB"))
                            itemAttribute.agi += 1;
                    }
                }
                break;
            case "ModuleB2":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleB"))
                            itemAttribute.agi += 1;
                    }
                }
                break;
            case "ModuleC":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleB"))
                            itemAttribute.agi += 5;
                    }
                }
                break;
            case "ModuleD1":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleD"))
                            itemAttribute.str += 1;
                    }
                }
                break;
            case "ModuleD2":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleD"))
                            itemAttribute.str += 1;
                    }
                }
                break;
            case "ModuleE":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleE"))
                            itemAttribute.intl += 1;
                    }
                }
                break;
            case "ModuleF1":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleF"))
                            itemAttribute.intl += 1;
                    }
                }
                break;
            case "ModuleF2":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleF"))
                            itemAttribute.intl += 1;
                    }
                }
                break;
            case "ModuleH":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleH"))
                            itemAttribute.str += 2;
                    }
                }
                break;
            case "ModuleI1":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleI"))
                            itemAttribute.men += 1;
                    }
                }
                break;
            case "ModuleI2":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleI"))
                            itemAttribute.men += 1;
                    }
                }
                break;
            case "ModuleI3":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleI"))
                            itemAttribute.men += 1;
                    }
                }
                break;
            case "ModuleI4":
                {
                    foreach (ItemTetrisPlacedObject itemTetrisPlacedObject in InventoryTetris.Instance.GetNearbyObjectList(this))
                    {
                        if (itemTetrisPlacedObject.itemTetrisSO.nameString.Contains("ModuleI"))
                            itemAttribute.men += 1;
                    }
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public ItemTetrisSO GetItemTetrisSO()
    {
        return itemTetrisSO;
    }

    public ItemAttribute GetItemAttribute()
    {
        return itemAttribute;
    }


    public  List<Vector2Int> GetGridPositionList()
    {
        return itemTetrisSO.GetGridPositionList(origin, dir);
    }


    public  Vector2Int GetGridPosition()
    {
        return origin;
    }

    public  ItemTetrisSO.Dir GetDir()
    {
        return dir;
    }

    public override string ToString()
    {
        return itemTetrisSO.nameString;
    }


    public  void SetOrigin(Vector2Int origin)
    {
        this.origin = origin;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


}