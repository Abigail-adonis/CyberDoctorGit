using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetrisAssets : MonoBehaviour {


    public static InventoryTetrisAssets Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public List<ItemTetrisSO> itemTetrisSOList = null;

    public ItemTetrisSO ModuleA;
    public ItemTetrisSO ModuleB1;
    public ItemTetrisSO ModuleB2;
    public ItemTetrisSO ModuleC;
    public ItemTetrisSO ModuleD1;
    public ItemTetrisSO ModuleD2;
    public ItemTetrisSO ModuleE;
    public ItemTetrisSO ModuleF1;
    public ItemTetrisSO ModuleF2;
    public ItemTetrisSO Heart1;
    public ItemTetrisSO Heart2;
    public ItemTetrisSO Heart3;
    public ItemTetrisSO Heart4;
    public ItemTetrisSO Heart5;
    public ItemTetrisSO Heart6;
    public ItemTetrisSO Heart7;

    public ItemTetrisSO GetItemTetrisSOFromName(string itemTetrisSOName)
    {
        foreach (ItemTetrisSO itemTetrisSO in itemTetrisSOList)
        {
            if (itemTetrisSO.name == itemTetrisSOName)
            {
                return itemTetrisSO;
            }
        }
        return null;
    }


    public Sprite gridBackground;
    public Sprite gridBackground_2;
    public Sprite gridBackground_3;

    public Sprite ModuleASprite;
    public Sprite ModuleB1Sprite;
    public Sprite ModuleB2Sprite;
    public Sprite ModuleCSprite;
    public Sprite ModuleD1Sprite;
    public Sprite ModuleD2Sprite;
    public Sprite ModuleESprite;
    public Sprite ModuleF1Sprite;
    public Sprite ModuleF2Sprite;
    public Sprite Heart1Sprite;
    public Sprite Heart2Sprite;
    public Sprite Heart3Sprite;
    public Sprite Heart4Sprite;
    public Sprite Heart5Sprite;
    public Sprite Heart6Sprite;
    public Sprite Heart7Sprite;
    public Sprite ModuleGSprite;
    public Sprite ModuleHSprite;
    public Sprite ModuleI1Sprite;
    public Sprite ModuleI2Sprite;
    public Sprite ModuleI3Sprite;
    public Sprite ModuleI4Sprite;

    public Transform gridVisual;

}