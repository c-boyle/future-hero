using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    private string label;
    [TextArea(3,10)] public string[] sentences;
}
