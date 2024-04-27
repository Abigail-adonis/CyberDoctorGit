using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.IO;
using static ItemTetrisPlacedObject;

public class InventoryTetris : MonoBehaviour {

    public static InventoryTetris Instance { get; private set; }
    [SerializeField] private InventoryBagUI inventoryBagUI;
    [SerializeField] private InspectorUI inspectorUI;
    private InventoryBag inventoryBag;

    public event EventHandler<ItemTetrisPlacedObject> OnObjectPlaced;

    private TGrid<GridObject> grid;
    private RectTransform itemContainer;
    private bool canRotate = false;



    private void Awake() {
        Instance = this;

        int gridWidth = 20;
        int gridHeight = 10;
        float cellSize = 50f;

        grid = new TGrid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (TGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        //transform.Find("BackgroundTempVisual").gameObject.SetActive(false);

    }

    private void Start()
    {
        inventoryBag = new InventoryBag();
        inventoryBagUI.SetInventory(inventoryBag);
        inspectorUI.SetInventory(inventoryBag, this);
    }

    public class GridObject {

        private TGrid<GridObject> grid;
        private int x;
        private int y;
        public ItemTetrisPlacedObject placedObject;
        public bool isBusy;

        public GridObject(TGrid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
            isBusy = false;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(ItemTetrisPlacedObject placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetBusy()
        {
            this.isBusy = true;
        }

        public void ClearBusy()
        {
            this.isBusy = false;
        }

        public ItemTetrisPlacedObject GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

        public bool HasPlacedObject() {
            return placedObject != null;
        }

    }

    public TGrid<GridObject> GetGrid() {
        return grid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }


    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, ItemTetrisSO.Dir dir) {
        // Test Can Build
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        foreach (Vector2Int gridPosition in gridPositionList) {
            bool isValidPosition = IsValidGridPosition(gridPosition);
            if (!isValidPosition) {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                canPlace = false;
                break;
            }
        }

        if (canPlace) {
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild() || grid.GetGridObject(gridPosition.x, gridPosition.y).isBusy) {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace) {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            ItemTetrisPlacedObject placedObject = ItemTetrisPlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir, itemTetrisSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));

            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }
            OnObjectPlaced?.Invoke(this, placedObject);

            // Object Placed!
            return true;
        } else {
            // Object CANNOT be placed!
            return false;
        }
    }

    public void RemoveItemAt(ItemTetrisPlacedObject placedObject) {
        //ItemTetrisPlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();

        if (placedObject != null) {
            // Demolish
            placedObject.DestroySelf();

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    public void MoveItemToBag(ItemTetrisPlacedObject placedObject)
    {
        //ItemTetrisPlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();
        if (placedObject != null)
        {
            // Demolish
            placedObject.DestroySelf();

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
            ItemTetrisSO itemTetrisSO = ScriptableObject.CreateInstance<ItemTetrisSO>();
            itemTetrisSO.nameString = placedObject.GetItemTetrisSO().nameString;
            inventoryBag.AddItemTetrisSO(itemTetrisSO, 1);
        }
    }

    public void MoveAllItemToBag()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    MoveItemToBag(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }
    }

    public List<Vector2Int> GetNearbyPointList(ItemTetrisPlacedObject placedObject)
    {
        List<Vector2Int> vector2Ints = new();
        foreach (Vector2Int point in placedObject.GetGridPositionList())
        {
            if (IsValidGridPosition(point + new Vector2Int(1, 0)))
                vector2Ints.Add(point + new Vector2Int(1, 0));
            if (IsValidGridPosition(point + new Vector2Int(-1, 0)))
                vector2Ints.Add(point + new Vector2Int(-1, 0));
            if (IsValidGridPosition(point + new Vector2Int(0, 1)))
                vector2Ints.Add(point + new Vector2Int(0, 1));
            if (IsValidGridPosition(point + new Vector2Int(0, -1)))
                vector2Ints.Add(point + new Vector2Int(0, -1));
            if (IsValidGridPosition(point + new Vector2Int(1, 1)))
                vector2Ints.Add(point + new Vector2Int(1, 1));
            if (IsValidGridPosition(point + new Vector2Int(-1, 1)))
                vector2Ints.Add(point + new Vector2Int(-1, 1));
            if (IsValidGridPosition(point + new Vector2Int(-1, -1)))
                vector2Ints.Add(point + new Vector2Int(-1, -1));
            if (IsValidGridPosition(point + new Vector2Int(1, -1)))
                vector2Ints.Add(point + new Vector2Int(1, -1));
        }
        vector2Ints.RemoveAll(x => placedObject.GetGridPositionList().Contains(x));
        return vector2Ints;
    }

    public List<ItemTetrisPlacedObject> GetNearbyObjectList(ItemTetrisPlacedObject placedObject)
    {
        List<ItemTetrisPlacedObject> placedObjectList = new();
        List<Vector2Int> vector2Ints = GetNearbyPointList(placedObject);
        foreach (Vector2Int point in vector2Ints)
        {
            if (grid.GetGridObject(point.x, point.y).HasPlacedObject())
            {
                placedObjectList.Remove(grid.GetGridObject(point.x, point.y).GetPlacedObject());
                placedObjectList.Add(grid.GetGridObject(point.x, point.y).GetPlacedObject());
            }
        }
        return placedObjectList;
    }

    public ItemAttribute AttributeSum( )
    {
        List<ItemTetrisPlacedObject> placedObjectList = new();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                if (grid.GetGridObject(x, y).HasPlacedObject()) {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }
        ItemAttribute itemAttribute = new ItemAttribute();
        foreach (ItemTetrisPlacedObject obj in placedObjectList)
        {
            itemAttribute.str += obj.GetItemAttribute().str;
            itemAttribute.con += obj.GetItemAttribute().con;
            itemAttribute.intl += obj.GetItemAttribute().intl;
            itemAttribute.men += obj.GetItemAttribute().men;
            itemAttribute.agi += obj.GetItemAttribute().agi;
            itemAttribute.rct += obj.GetItemAttribute().rct;
            itemAttribute.cha += obj.GetItemAttribute().cha;
            itemAttribute.san += obj.GetItemAttribute().san;
            itemAttribute.price += obj.GetItemAttribute().price;
        }
        return itemAttribute;
    }
    public bool CheckBalance()
    {
        List<ItemTetrisPlacedObject> placedObjectList1 = new();
        List<ItemTetrisPlacedObject> placedObjectList2 = new();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    if (x < grid.GetWidth() / 2)
                    {
                        placedObjectList1.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                        placedObjectList1.Add(grid.GetGridObject(x, y).GetPlacedObject());
                    }
                    else
                    {
                        placedObjectList2.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                        placedObjectList2.Add(grid.GetGridObject(x, y).GetPlacedObject());
                    }
                }
            }
        }
        ItemAttribute itemAttribute1 = new ItemAttribute();
        foreach (ItemTetrisPlacedObject obj in placedObjectList1)
        {
            itemAttribute1.str += obj.GetItemAttribute().str;
            itemAttribute1.con += obj.GetItemAttribute().con;
            itemAttribute1.intl += obj.GetItemAttribute().intl;
            itemAttribute1.men += obj.GetItemAttribute().men;
            itemAttribute1.agi += obj.GetItemAttribute().agi;
            itemAttribute1.rct += obj.GetItemAttribute().rct;
            itemAttribute1.cha += obj.GetItemAttribute().cha;
            itemAttribute1.san += obj.GetItemAttribute().san;
            itemAttribute1.price += obj.GetItemAttribute().price;
        }

        ItemAttribute itemAttribute2 = new ItemAttribute();
        foreach (ItemTetrisPlacedObject obj in placedObjectList2)
        {
            itemAttribute2.str += obj.GetItemAttribute().str;
            itemAttribute2.con += obj.GetItemAttribute().con;
            itemAttribute2.intl += obj.GetItemAttribute().intl;
            itemAttribute2.men += obj.GetItemAttribute().men;
            itemAttribute2.agi += obj.GetItemAttribute().agi;
            itemAttribute2.rct += obj.GetItemAttribute().rct;
            itemAttribute2.cha += obj.GetItemAttribute().cha;
            itemAttribute2.san += obj.GetItemAttribute().san;
            itemAttribute2.price += obj.GetItemAttribute().price;
        }
        if (itemAttribute1.str != itemAttribute2.str
            || itemAttribute1.con != itemAttribute2.con 
            || itemAttribute1.intl != itemAttribute2.intl
            || itemAttribute1.men != itemAttribute2.men
            || itemAttribute1.agi != itemAttribute2.agi
            || itemAttribute1.rct != itemAttribute2.rct
            || itemAttribute1.cha != itemAttribute2.cha)
            return false;
        return true;
    }

    public void SetGridBusy(List<Vector2Int> vector2IntList)
    {
        RemoveAllBusy();
        foreach(Vector2Int vector2Int in vector2IntList)
        {
            grid.GetGridObject(vector2Int.x, vector2Int.y).SetBusy();
        }
    }

    public void RemoveAllBusy()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                grid.GetGridObject(x, y).ClearBusy();
            }
        }
    }

    public void RefreshAllAttribute()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    grid.GetGridObject(x, y).GetPlacedObject().RefreshAttribute();
                }
            }
        }
    }
    public void  SetRotationState(bool rotate)
    {
        canRotate = rotate;
    }

    public bool GetRotationState()
    {
        return canRotate;
    }

    public RectTransform GetItemContainer() {
        return itemContainer;
    }

    public InventoryBag GetInventoryBag()
    {
        return inventoryBag;
    }



    //[Serializable]
    //public struct AddItemTetris
    //{
    //    public string itemTetrisSOName;
    //    public int itemTetrisCount;
    //}

    //[Serializable]
    //public struct ListAddItemTetris {
    //    public List<AddItemTetris> addItemTetrisList;
    //}

    //public string Save() {
    //    List<ItemTetrisPlacedObject> placedObjectList = new List<ItemTetrisPlacedObject>();
    //    for (int x = 0; x < grid.GetWidth(); x++) {
    //        for (int y = 0; y < grid.GetHeight(); y++) {
    //            if (grid.GetGridObject(x, y).HasPlacedObject()) {
    //                placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
    //                placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
    //            }
    //        }
    //    }

    //    List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
    //    foreach (ItemTetrisPlacedObject placedObject in placedObjectList) {
    //        addItemTetrisList.Add(new AddItemTetris {
    //            dir = placedObject.GetDir(),
    //            gridPosition = placedObject.GetGridPosition(),
    //            itemTetrisSOName = placedObject.GetItemTetrisSO().name,
    //        });

    //    }

    //    string json = JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    //    return json;
    //}

    //public void Load(string loadString) {
    //    ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

    //    foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList) {
    //        TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition, addItemTetris.dir);
    //    }
    //}

    [Serializable]
    public struct ListAddItemTetrisMap
    {
        public List<Vector2Int> map;
        public List<string> nameList;
        public int leveltype;
        public ItemTetrisPlacedObject.ItemAttribute itemAttribute;

    }

    [Serializable]
    public struct ListAddJson
    {
        public int saveNum;
    }
    public void saveJsonNum(int num)
    {
        string json = JsonUtility.ToJson(new ListAddJson { saveNum = num });
        string path = Application.streamingAssetsPath + "/jsonData.json";
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
        Debug.Log(num); 
    } 

    public int LoadJsonNum()
    {
        int loadNum = 0;
        string json;
        string path = Application.streamingAssetsPath + "/jsonData.json";
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            ListAddJson jsonNum = JsonUtility.FromJson<ListAddJson>(json);
            loadNum = jsonNum.saveNum;
        }
        return loadNum;
    }


    public string SaveExceptJsonMap(int mapType)
    {
        List<Vector2Int> pointList = new();
        List<ItemTetrisPlacedObject> itemTetrisList = new();
        int saveNum = LoadJsonNum();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                pointList.Add(new Vector2Int(x, y));
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    pointList.Remove(new Vector2Int(x, y));
                    itemTetrisList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    itemTetrisList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<string> addItemTetrisList = new List<string>();
        foreach (ItemTetrisPlacedObject item in itemTetrisList)
            addItemTetrisList.Add(item.ToString());
        ItemTetrisPlacedObject.ItemAttribute attribute = new()
        {
            str = 10,
            con = 0,
            intl = 0,
            men = 0,
            agi = 0,
            rct = 0,
            cha = 0,
            san = 0,
            price = 0,
        };
        string json = JsonUtility.ToJson(new ListAddItemTetrisMap { map = pointList, leveltype = mapType, itemAttribute = attribute ,nameList = addItemTetrisList});
        string path = Application.streamingAssetsPath + "/mapData" + saveNum.ToString() + ".json";
        saveJsonNum(++saveNum);
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
        return json;
    }

    public void loadJsonMap(int selectNum,out List<Vector2Int> vector2Ints, out int type, out ItemTetrisPlacedObject.ItemAttribute attribute, out List<string> strings)
    {
        vector2Ints = new ();
        type = 0;
        attribute = new();
        strings = new List<string>();
        string json;
        string path = Application.streamingAssetsPath + "/mapData" + selectNum.ToString() + ".json";
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            ListAddItemTetrisMap listAddItemTetris = JsonUtility.FromJson<ListAddItemTetrisMap>(json);
            foreach (Vector2Int vector2Int in listAddItemTetris.map)
            {
                vector2Ints.Add(vector2Int);
            }
            type = listAddItemTetris.leveltype;
            attribute = listAddItemTetris.itemAttribute;
            strings = listAddItemTetris.nameList;
        }
    }
}
