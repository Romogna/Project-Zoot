﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class InstructionsG
{
    public string name;

    [TextArea(3, 10)]
    public string[] instructions;
}
