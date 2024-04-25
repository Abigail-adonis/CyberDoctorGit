using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CyberDoctor.Dialogue;

public class DialogueOverride
{
    public string npcName;
    public DialoguePiece dialogue;
    public int? requiredLevelProgress;
    public int? requiredAffectionLevel;

    public DialogueOverride(string name, DialoguePiece dlg, int? lvlProgress = null, int? affLevel = null)
    {
        npcName = name;
        dialogue = dlg;
        requiredLevelProgress = lvlProgress;
        requiredAffectionLevel = affLevel;
    }

    public bool IsOverrideActive(NPCData npcData)
    {
        if (requiredLevelProgress.HasValue && requiredLevelProgress.Value != npcData.levelProgress)
            return false;
        if (requiredAffectionLevel.HasValue && requiredAffectionLevel.Value != npcData.affection)
            return false;
        return true;
    }
}
