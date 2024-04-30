using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace CyberDoctor.Dialogue
{
    [System.Serializable]

    public class DialoguePiece
    { 
        [Header("对话详情")]
        public Sprite faceImage;
        public bool onLeft;
        public string name;
        [TextArea]

        public string dialogueText;
        public bool hasToPause;
        public List<DialogueOption> options; // 选项列表
        public DialogueOption selectedOption; // 存储玩家选择的选项

        [HideInInspector]public bool isDone;    
    }

    [System.Serializable]
        public class DialogueOption
        {
            public string text;
            public string action;
            public string levelSceneName;// 如果action是"Teleport"，这里是场景名
            //public UnityEvent onSelectEvent;
            public DialoguePiece NextPiece;  // 指向下一个对话片段的引用
        }
}
