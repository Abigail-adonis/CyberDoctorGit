using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CyberDoctor.Dialogue;

public static class EventHandlers
{
    // 事件声明
    public static event Action<DialoguePiece> ShowDialogueEvent;
    public static event Action<List<DialogueOption>> OnShowOptionsEvent;  // 添加选项显示事件的声明
    public static event Action<DialogueOption> OnOptionSelected;

    public static void RaiseOptionSelected(DialogueOption option)
    {
        OnOptionSelected?.Invoke(option);
    }
    // 调用显示对话事件
    public static void CallShowDialogueEvent(DialoguePiece piece)
    {
        ShowDialogueEvent?.Invoke(piece);
    }

    // 添加显示选项事件的方法
    public static void CallShowOptionsEvent(List<DialogueOption> options)
    {
        OnShowOptionsEvent?.Invoke(options);
    }
}
