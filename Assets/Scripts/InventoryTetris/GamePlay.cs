using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;
    [SerializeField] public Teleport teleporter; // Teleport �ű�������
    [SerializeField] private Mainpanel mainpanel;
    [SerializeField] private InspectorUI inspectorUI;



    List<Vector2Int> maps;
    List<string> nameLists = new();
    int mapNum = 0;
    int mapType = 0;//�ؿ����� 0�����Լ��� 1��ƴͼ
    ItemTetrisPlacedObject.ItemAttribute itemAttribute;
    string specialRule;
    // Start is called before the first frame update
    void Start()
    {
        //Ԥ�������Start���ȡ��ɫ����Ϣ�͹ؿ���Ϣ���عؿ�����Ҫ��projectsetting ���ֶ���gameplay�ű�������˳���������
        mapNum = 0;//PlayerPrefs.GetInt("Character",0) + PlayerPrefs.GetInt("Level",0);
        LoadLevel();
    }

    void Awake()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //�����ô��룬�����������ɹؿ�,���ؿ���������Ϊjson�ļ��洢��Assets/Sreamingassets��
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(inventoryTetris.SaveExceptJsonMap(0));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(inventoryTetris.SaveExceptJsonMap(1));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            inventoryTetris.saveJsonNum(0);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            inventoryTetris.loadJsonMap(mapNum,out maps, out mapType, out itemAttribute, out nameLists, out specialRule);
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

    private void LoadLevel()
    {
        inventoryTetris.loadJsonMap(mapNum, out maps, out mapType, out itemAttribute, out nameLists, out specialRule);
        inventoryTetrisBackground.ChangeBackgroundColor(maps);
        inventoryTetris.SetGridBusy(maps);
        inspectorUI.SetAttributeCheck(itemAttribute, specialRule);
        if (nameLists.Count > 0)
        {
            inventoryTetris.GetInventoryBag().ClearAllItemAmout();
            inventoryTetris.GetInventoryBag().SetItemNum(nameLists);
        }
        else
        {
            inventoryTetris.GetInventoryBag().ReloadItem();
        }
    }

    public void TeleportToNextLevel()
    {
        // ��������Ҫ���ݶԻ�������������͵��ĸ��ؿ�
        string levelSceneName = "PersistantScene"; // ����ʵ�ʶԻ�������ùؿ�����
        teleporter.TeleportToLevel(levelSceneName);
    }

    public void TestToNextLevel()
    {
        inventoryTetris.MoveAllItemToBag();
        mainpanel.gameObject.SetActive(false);
        mapNum++;
        mapNum = mapNum % 7;
        LoadLevel();

    }

    public void SubmitGame()
    {
        //����ؿ������ݹؿ����ͽ��ж�Ӧ�Ľ��㣬ͳ������ֵ����ƴͼ�Ƿ�����
        switch (mapType)
        {
            case 0:
                if (CaculaterAttrribute())
                {
                    mainpanel.gameObject.SetActive(true);
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
                    mainpanel.gameObject.SetActive(true);
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
            || itemAttribute.cha > attributeSum.cha
            || itemAttribute.san < attributeSum.san)
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
