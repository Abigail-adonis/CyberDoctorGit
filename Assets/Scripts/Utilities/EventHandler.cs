using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CyberDoctor.Dialogue;

public static class EventHandler
{
    public static event Action<DialoguePiece> ShowDialogueEvent;

    public static void CallShowDialogueEvent(DialoguePiece piece)
    {
        // 确保ShowDialogueEvent已经被赋值
        if (ShowDialogueEvent != null)
        {
            ShowDialogueEvent.Invoke(piece);
        }
    }
}
