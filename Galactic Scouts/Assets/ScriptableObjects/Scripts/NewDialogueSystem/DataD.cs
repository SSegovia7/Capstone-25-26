using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DataD :ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        public string characterName;
        public string tx;
        public Sprite spr;
        public int action;
    }

   
    public DialogueLine[] lines;
}
