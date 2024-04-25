using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string npcName;
    public int levelProgress;
    public int affection;

    public NPCData(string name, int progress, int aff)
    {
        npcName = name;
        levelProgress = progress;
        affection = aff;
    }

    public void NPCAff(int aff)
    {
        if(aff>0){
            affection+=1;
        }
        else{
            affection-=99999;
        }
    }

    public bool CanAdvanceLevel => levelProgress > 0 && affection >= 0;
}
