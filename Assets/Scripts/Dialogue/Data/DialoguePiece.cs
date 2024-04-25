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
        public List<Option> options;//选项信息 

        [HideInInspector]public bool isDone;

        [System.Serializable]
        public class Option
        {
            public string text;
            public UnityEvent onSelectEvent;
        }
    }
}
