using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBag
{
    public class Item
    {
        public string nameString;
        public string descriptionString;

        public int Str;//����
        public int Con;//����
        public int Agi;//����
        public int Int;//����
        public int Men;//����
        public int Cha;//����
        public int Rct;//��Ӧ
        public int San;//����
        public int Price;//�۸�
        public int amount;
        public int pointCount;

        public Item(string nameString, string descriptionString, int str, int con, int @int, int men, int agi, int rct, int cha, int san, int price, int amount, int pointCount)
        {
            this.nameString = nameString;
            this.descriptionString = descriptionString;
            Str = str;
            Con = con;
            Int = @int;
            Men = men;
            Agi = agi;
            Rct = rct;
            Cha = cha;
            San = san;
            Price = price;
            this.amount = amount;
            this.pointCount = pointCount;
        }
    }
    public  event EventHandler OnInventoryBagChange;
    public List<Item> itemList;

    public InventoryBag()
    {
        itemList = new List<Item>();
        foreach (ItemTetrisSO itemTetrisSO in InventoryTetrisAssets.Instance.itemTetrisSOList)
        {
            AddItemTetrisSO(itemTetrisSO, 0);
        }
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleA,10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleB1, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleB2, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleC, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleE, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleD1, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleD2, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleF1, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.ModuleF2, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart1, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart2, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart3, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart4, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart5, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart6, 10);
        //AddItemTetrisSO(InventoryTetrisAssets.Instance.Heart7, 10);
    }

    public void AddItemTetrisSO(ItemTetrisSO itemTetrisSO, int num)
    {
        bool itemAlreadyInList = false;
        foreach (Item item in itemList)
        { 
            if(item.nameString == itemTetrisSO.nameString)
            {
                item.amount += num;
                itemAlreadyInList = true;
            }
        }
        if (!itemAlreadyInList)
        {
            itemList.Add(new Item (itemTetrisSO.nameString, itemTetrisSO.descriptionString, itemTetrisSO.Str, itemTetrisSO.Con, itemTetrisSO.Int, itemTetrisSO.Men, 
                itemTetrisSO.Agi, itemTetrisSO.Rct, itemTetrisSO.Cha, itemTetrisSO.San, itemTetrisSO.Price, itemTetrisSO.amount, itemTetrisSO.points.Count));
        }
        OnInventoryBagChange?.Invoke(this, EventArgs.Empty);
    }

    public bool RemoveItemTetrisSO(ItemTetrisSO itemTetrisSO, int num)
    {
        bool itemNoMore = false;
        Item itemInList = null;
        foreach (Item item in itemList)
        {
            if (item.nameString == itemTetrisSO.nameString)
            {
                item.amount -= num;
                itemInList = item;
            }
        }
        if (itemInList != null && itemInList.amount <= 0)
        {
            itemNoMore = true;
        }
        OnInventoryBagChange?.Invoke(this, EventArgs.Empty);
        return itemNoMore;
    }

    public List<Item> GetItemTetrisList()
    {
        return itemList;
    }

    public void SetItemDataToSO()
    {
        foreach (Item item in itemList)
        {
            InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(item.nameString).amount = item.amount;
        }
    }

    public void ClearAllItemAmout()
    {
        foreach (Item item in itemList)
            item.amount = 0;
        OnInventoryBagChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetItemNum(List<string> sList)
    {
        foreach(string s in sList)
        {
            AddItemTetrisSO(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(s), 1);
        }
    }

    public void ReloadItem()
    {
        foreach (Item item in itemList)
        {
            item.amount = InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(item.nameString).amount ;
        }
        OnInventoryBagChange?.Invoke(this, EventArgs.Empty);
    }
}
