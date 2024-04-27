using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;


    List<Vector2Int> maps;
    List<string> nameLists = new();
    int mapNum = 0;
    int mapType = 0;//关卡类型 0：属性检验 1：拼图 2：特殊处理，检查两边是否属性一致
    ItemTetrisPlacedObject.ItemAttribute itemAttribute;
    // Start is called before the first frame update
    void Start()
    {
        //预想的是在Start里读取角色的信息和关卡信息加载关卡，需要在projectsetting 内手动把gameplay脚本的运行顺序调到后面
        //关卡信息用json存储，用角色信息和关卡进度计算出读取的文件
        //mapNum = PlayerPrefs.GetInt("Character",0) + PlayerPrefs.GetInt("Level",0);
        //inventoryTetris.loadJsonMap(mapNum, out maps, out mapType, out itemAttribute, out nameLists);
        //inventoryTetrisBackground.ChangeBackgroundColor(maps);
        //inventoryTetris.SetGridBusy(maps);
        //if (nameLists.Count > 0)
        //{
        //    inventoryTetris.GetInventoryBag().ClearAllItemAmout();
        //    inventoryTetris.GetInventoryBag().SetItemNum(nameLists);
        //}
        //else
        //{
        //    inventoryTetris.GetInventoryBag().ReloadItem();
        //}
    }

    void Awake()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //测试用代码，可以用来生成关卡,将关卡数据生成为json文件存储在Assets/Sreamingassets中，文件名从mapjson0,mapjson1递增
        //S键存储，把当前关卡内占用的格子作为关卡的空格存储，并记录对应的积木块，属性值，还需打开json修改准确的信息
        //O键，将文件序号重置到0，下次存储重新从mapjson0开始
        //L键，读取关卡信息，读取规则可变，目前为json0-json6循环读取
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(inventoryTetris.SaveExceptJsonMap(0));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            inventoryTetris.saveJsonNum(0);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            inventoryTetris.loadJsonMap(mapNum,out maps, out mapType, out itemAttribute, out nameLists);
            inventoryTetrisBackground.ChangeBackgroundColor(maps);
            inventoryTetris.SetGridBusy(maps);
            if (nameLists.Count > 0)
            {
                inventoryTetris.GetInventoryBag().ClearAllItemAmout();
                inventoryTetris.GetInventoryBag().SetItemNum(nameLists);
            }
            else
            {
                inventoryTetris.GetInventoryBag().ReloadItem();
            }
            mapNum++;
            mapNum = mapNum % 7;
        }
    }

    public void SubmitGame()
    {
        //结算关卡，根据关卡类型进行对应的结算，统计属性值或检查拼图是否完整
        //预计在这块添加返回给剧情系统的数据
        switch (mapType)
        {
            case 0:
                if (CaculaterAttrribute())
                {
                    TooltipCanvas.ShowTooltip_Static("Success!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }
                else
                {
                    TooltipCanvas.ShowTooltip_Static("Fail!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }
                break;
            case 1:
                if (CheckFull())
                {
                    TooltipCanvas.ShowTooltip_Static("Success!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }
                else
                {
                    TooltipCanvas.ShowTooltip_Static("Fail!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }
                break;
            case 2:
                if (CaculaterAttrribute()&& inventoryTetris.CheckBalance())
                {
                    TooltipCanvas.ShowTooltip_Static("Success!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }
                else
                {
                    TooltipCanvas.ShowTooltip_Static("Fail!");
                    FunctionTimer.Create(() => { TooltipCanvas.HideTooltip_Static(); }, 2f, "HideTooltip", true, true);
                }   
                break;
        }
    }

    private bool CaculaterAttrribute()
    {
        ItemTetrisPlacedObject.ItemAttribute attributeSum = inventoryTetris.AttributeSum();
        if (itemAttribute.str > attributeSum.str 
            || itemAttribute.con > attributeSum.con 
            || itemAttribute.intl > attributeSum.intl 
            || itemAttribute.men > attributeSum.men
            || itemAttribute.agi > attributeSum.agi
            || itemAttribute.rct > attributeSum.rct
            || itemAttribute.cha > attributeSum.cha)
            return false;
        return true;
    } 

    private bool CheckFull()
    {
        for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++)
        {
            for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++)
            {
                if (!(inventoryTetris.GetGrid().GetGridObject(x,y).HasPlacedObject() || inventoryTetris.GetGrid().GetGridObject(x,y).isBusy))
                {
                    //Debug.Log(x + ","+y);
                    return false;
                }
            }
        }
        return true;
    }


}
