using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueNode : Node {
    public string GUID;
    public string dialogueText;
    public bool isEntryPoint = false;
}
