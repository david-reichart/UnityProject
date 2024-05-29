using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainerScriptableObject : ScriptableObject {
    public List<DialogueNodeData> dialogueNodesData = new List<DialogueNodeData>();
    public List<DialogueNodeLinkData> nodeLinks = new List<DialogueNodeLinkData>();
}
