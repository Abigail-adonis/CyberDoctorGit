using System.Collections.Generic;
using CyberDoctor.Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    [SerializeField]
    private List<NPCData> npcDataList = new List<NPCData>();

    [SerializeField]
    private List<DialogueOverride> dialogueOverrides = new List<DialogueOverride>();

    [SerializeField]
    private List<NPCDialogueData> npcDialogueLists;

    // 辅助类，用于存储每个NPC的对话数据
    [System.Serializable]
    public class NPCDialogueData
    {
        public string npcName;
        public List<DialoguePiece> dialogues = new List<DialoguePiece>();
    }

    // 使用 Dictionary 存储 NPC 数据，以便在其他方法中使用
    private Dictionary<string, NPCData> npcDataDict;
    private Dictionary<string, List<DialoguePiece>> npcDialogues;

    // Awake() 用于初始化单例
    private void Awake()
    {
        // 确保只有一个NPCManager实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 设置为在场景切换时不销毁
            InitializeNPCData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 如果实例已存在，销毁当前对象
        }
        // 初始化对话数据存储
        npcDialogues = new Dictionary<string, List<DialoguePiece>>();
        // 填充对话数据到字典中
        LoadNPCDialogues();
    }

    private void Start()
    {
        // 游戏开始时，加载初始对话
        LoadInitialDialogues();
    }

    // 初始化NPC数据字典
    private void InitializeNPCData()
    {
        npcDataDict = new Dictionary<string, NPCData>();
        foreach (var data in npcDataList)
        {
            // 尝试添加NPC数据到字典中，如果键已存在，则会覆盖旧值
            npcDataDict[data.npcName] = data;
        }
    }

    public void LoadInitialDialogues()
    {
        // 为每个NPC加载初始对话
        foreach (var npcName in npcDataDict.Keys)
        {
            var dialogue = GetDialogueForNPC(npcName);
            // 根据dialogue执行显示对话的逻辑
        }
    }

    private void LoadNPCDialogues()
    {
        // 假设 npcDialogueLists 是一个已经存在的列表，其中包含了所有 NPC 的对话数据
        foreach (var npcDialogueData in npcDialogueLists)
        {
            // 将每个 NPC 的对话数据添加到 npcDialogues 字典中
            npcDialogues.Add(npcDialogueData.npcName, npcDialogueData.dialogues);
        }
    }

    public List<DialoguePiece> GetNPCDialogues(string npcName)
    {
        foreach (var npcData in npcDialogueLists)
        {
            if (npcData.npcName == npcName)
            {
                return npcData.dialogues;
            }
        }
        return null; // 如果NPC名称不存在，返回null
    }

    /// <summary>
    /// 获得不同分歧的对话
    /// </summary>
    public DialoguePiece GetDialogueForNPC(string npcName)
    {
        if (npcDataDict.TryGetValue(npcName, out NPCData npcData))
        {
            foreach (DialogueOverride overrideRule in dialogueOverrides)
            {
                if (overrideRule.npcName == npcName && overrideRule.IsOverrideActive(npcData))
                {
                    return overrideRule.dialogue;
                }
            }

            if (npcData.affection < 0)
            {
                return LoadFailureDialogue(npcName);
            }
            else if (npcData.CanAdvanceLevel)
            {
                DialoguePiece successDialogue = LoadSuccessDialogue(npcName, npcData.levelProgress);
                return successDialogue;
            }
            else
            {
                return LoadDefaultDialogue(npcName);
            }
        }
        return null;
    }

    private DialoguePiece LoadFailureDialogue(string npcName)
    {
        return new DialoguePiece
        {
            faceImage = null,
            onLeft = false,
            name = npcName,
            dialogueText = "失败剧情 " + npcName
        };
    }

    private DialoguePiece LoadSuccessDialogue(string npcName, int levelProgress)
    {
        return new DialoguePiece
        {
            faceImage = null,
            onLeft = false,
            name = npcName,
            dialogueText = "成功剧情 " + levelProgress
        };
    }

    private DialoguePiece LoadDefaultDialogue(string npcName)
    {
        return new DialoguePiece
        {
            faceImage = null,
            onLeft = false,
            name = npcName,
            dialogueText = "其他剧情" + npcName
        };
    }

    public void AdvanceLevel(string npcName, int newLevel)
    {
        if (npcDataDict.ContainsKey(npcName))
        {
            npcDataDict[npcName].levelProgress = newLevel;
        }
    }

    public void ChangeAffection(string npcName, int newAffection)
    {
        if (npcDataDict.ContainsKey(npcName))
        {
            npcDataDict[npcName].affection = newAffection;
        }
    }


    public void OnDialogueFinished(string npcName)
    {
        if (npcDataDict[npcName].CanAdvanceLevel)
        {
            SceneManager.LoadScene("Level" + npcDataDict[npcName].levelProgress);
        }
    }
}
