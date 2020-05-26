using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueG // Dialogue = Emery InstructionsG
{
    // public string name; Emery code

    [TextArea(3, 10)]
    public string[] sentences; // sentences = Emery "instructions"

}
