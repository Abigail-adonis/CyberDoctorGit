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

        //public UnityEvent OnFinishEvent;

        private Stack<DialoguePiece> dialogueStack;

        private bool canTalk;
        private bool isTalking;

        private List<string> npcNames = new List<string>
        {
            //"BigMan",
            "Woman",
            //"Man",
        };

        public Teleport teleporter; // Teleport 脚本的引用
        public UnityEvent OnDialogueFinished; // 对话结束事件

        private void Start()
        {
            // 订阅对话结束事件
            OnDialogueFinished.AddListener(TeleportToNextLevel);
            EventHandlers.OnOptionSelected += HandleOptionSelection;
        }

        private void OnEnable()
        {
            if (button != null)
            {
                // 使用 GetRandomNPCName 来传递一个随机选择的NPC名字给 OnButtonClick
                button.onClick.AddListener(() => OnButtonClick(GetRandomNPCName()));
            }
        }

        private void OnDestroy()
        {
            // 取消订阅对话结束事件
            OnDialogueFinished.RemoveListener(TeleportToNextLevel);
            EventHandlers.OnOptionSelected -= HandleOptionSelection;
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

        private void TeleportToNextLevel()
        {
            // 假设我们要根据对话结果来决定传送到哪个关卡
            string levelSceneName = "SampleScene"; // 根据实际对话结果设置关卡名称
            teleporter.TeleportToLevel(levelSceneName);
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
                DialoguePiece nextPiece = dialogueStack.Pop();
                StartCoroutine(ShowDialogue(nextPiece));  // 显示对话
            }
        }

        // 协程来显示对话
        private IEnumerator ShowDialogue(DialoguePiece piece)
        {
            EventHandlers.CallShowDialogueEvent(piece);  // 告知UI显示新的对话内容
            yield return new WaitUntil(() => piece.isDone);  // 等待对话完成

            // 检查是否有后续对话
            if (dialogueStack.Count > 0)
            {
                StartDialogue();  // 继续下一个对话
            }
            else
            {
                OnDialogueFinished?.Invoke();  // 所有对话完成
            }
        }
        private void HandleOptionSelection(DialogueOption selectedOption)
        {
            if (selectedOption.NextPiece != null)
            {
                dialogueStack.Clear();  // 清空当前对话栈
                dialogueStack.Push(selectedOption.NextPiece);  // 将选中选项的后续对话推入对话栈
                StartDialogue();  // 开始新的对话
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
                    EventHandlers.CallShowDialogueEvent(result);
                    yield return new WaitUntil(() => result.isDone); // 直到对话结束，isTalking变为false
                    // 检查是否有选项事件
                    if (result.options != null && result.options.Count > 0)
                    {
                        // 显示选项并等待玩家做出选择
                        EventHandlers.CallShowOptionsEvent(result.options);
                        yield return new WaitUntil(() => result.selectedOption != null); // 等待选择
        
                        // 处理选项选择
                        if (result.selectedOption.action == "ContinueDialogue")
                        {
                            continue; // 继续下一轮对话
                        }
                        else if (result.selectedOption.action == "Teleport")
                        {
                            teleporter.TeleportToLevel(result.selectedOption.levelSceneName);
                            yield break; // 退出对话协程
                        }
                    }
                }
                isTalking = false;
            }

            // 所有对话结束
            EventHandlers.CallShowDialogueEvent(null);
            //OnFinishEvent?.Invoke();
            //OnDialogueFinished.Invoke(); // 触发场景跳转事件
            ButtonCanvas.SetActive(true);// 对话结束后，重新使按钮可交互
        }
    }
}