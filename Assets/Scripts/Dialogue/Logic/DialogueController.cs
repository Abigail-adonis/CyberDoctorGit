using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CyberDoctor.Dialogue{
    public class DialogueController : MonoBehaviour
    {
        public Button button;
        public GameObject ButtonCanvas;

        public UnityEvent OnFinishEvent;

        private Stack<DialoguePiece> dialogueStack;

        private bool canTalk;
        private bool isTalking;

        private List<string> npcNames = new List<string>
        {
            "BigMan",
            "Woman",
            "Man",
        };

        private void OnEnable()
        {
            if (button != null)
            {
                // 使用 GetRandomNPCName 来传递一个随机选择的NPC名字给 OnButtonClick
                button.onClick.AddListener(() => OnButtonClick(GetRandomNPCName()));
            }
        }

        private void OnDisable()
        {
            // 取消监听
            if (button != null)
            {
                button.onClick.RemoveListener(() => OnButtonClick(GetRandomNPCName()));
            }
        }

        private void OnButtonClick(string npcName)
        {
            ButtonCanvas.SetActive(false); // 对话开始时隐藏按钮画布
            StartNPCDialogue(npcName); // 使用NPC的名字开始对话
        }

        private string GetRandomNPCName()
        {
            // 随机选择一个NPC名字
            if (npcNames.Count > 0) // 确保列表不为空
            {
                return npcNames[Random.Range(0, npcNames.Count)];
            }
            return null; // 如果列表为空，返回null
        }

        public void StartNPCDialogue(string npcName)
        {
            List<DialoguePiece> npcDialogues = NPCManager.Instance.GetNPCDialogues(npcName);
            if (npcDialogues != null)
            {
                // 将获取到的对话数据转换为栈
                dialogueStack = new Stack<DialoguePiece>(npcDialogues);
                foreach (var dialogue in dialogueStack)
                {
                   dialogue.isDone = false;
                }
                StartDialogue();
            }
            else
            {
                Debug.LogError("No dialogues found for NPC: " + npcName);
            }
        }

        private void StartDialogue()
        {
            if (dialogueStack.Count > 0)
            {
                StartCoroutine(DialogueRoutine());
            }
        }

        private IEnumerator DialogueRoutine()
        {
            while (dialogueStack.Count > 0)
            {
                isTalking = true;
                if (dialogueStack.TryPop(out DialoguePiece result))
                {
                    // 传到UI显示对话
                    EventHandler.CallShowDialogueEvent(result);
                    yield return new WaitUntil(() => result.isDone); // 直到对话结束，isTalking变为false
                    // 检查是否有选项事件
                    if (result.options != null && result.options.Count > 0)
                    {
                        // 这里可以添加逻辑来处理选项，例如：
                        // - 触发一个特定的选项事件
                        // - 根据玩家选择更改对话流程
                    }
                }
                isTalking = false;
            }

            // 所有对话结束
            EventHandler.CallShowDialogueEvent(null);
            OnFinishEvent?.Invoke();
            ButtonCanvas.SetActive(true);// 对话结束后，重新使按钮可交互
        }
    }
}