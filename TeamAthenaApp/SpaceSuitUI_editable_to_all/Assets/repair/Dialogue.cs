 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue // Dialogue = Emery InstructionsG
{
    [TextArea(3, 10)]
    public string[] sentences; // sentences = Emery "instructions"
}

[System.Serializable]
public class Geology // Dialogue = Emery InstructionsG
{
    // For Tire changing instructions
    [TextArea(3, 10)]
    public string[] instructions;
}
