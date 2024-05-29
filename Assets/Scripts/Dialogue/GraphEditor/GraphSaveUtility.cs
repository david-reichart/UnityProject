using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class GraphSaveUtility {
    private DialogueGraphView _targetGraphView;
    private List<Edge> edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    private DialogueContainerScriptableObject dialogueContainerScriptableObject;
        
    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) {
        return new GraphSaveUtility {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName) {
        if (!this.edges.Any()) {
            return;
        }

        DialogueContainerScriptableObject dialogueContainer = ScriptableObject.CreateInstance<DialogueContainerScriptableObject>();
        // only save output port information
        // if an output port is connected to an input port, that information is saved
        Edge[] connectedPorts = this.edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++) {
            DialogueNode outputNode = connectedPorts[i].output.node as DialogueNode;
            DialogueNode inputNode = connectedPorts[i].input.node as DialogueNode;
            
            dialogueContainer.nodeLinks.Add(new DialogueNodeLinkData {
                baseNodeGuid = outputNode.GUID,
                connectingPortName = connectedPorts[i].output.portName,
                targetNodeGuid = inputNode.GUID
            });
        }

        foreach (DialogueNode node in this.nodes) {
            if (node.isEntryPoint == false) {
                dialogueContainer.dialogueNodesData.Add(new DialogueNodeData {
                    nodeGuid = node.GUID,
                    dialogueText = node.dialogueText,
                    nodePositionOnGraph = node.GetPosition().position
                });
            }
        }
        
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Scripts/Dialogue/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName) {
        this.dialogueContainerScriptableObject = Resources.Load<DialogueContainerScriptableObject>(fileName);
        if (this.dialogueContainerScriptableObject == null) {
            EditorUtility.DisplayDialog(
                "File not found",
                "Target file was not found. Check to make sure the correct file name was used.",
                "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph() {
        // the first node link in the saved file will always be the entry point
        this.nodes.Find(x => x.isEntryPoint).GUID = dialogueContainerScriptableObject.nodeLinks[0].baseNodeGuid;
        foreach (DialogueNode node in this.nodes) {
            if (node.isEntryPoint) {
                continue;
            }
            else {
                // remove edges connected to this node
                this.edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
                // remove the node
                _targetGraphView.RemoveElement(node);
            }
        }
    }

    private void CreateNodes() {
        foreach (DialogueNodeData nodeData in this.dialogueContainerScriptableObject.dialogueNodesData) {
            DialogueNode tempNode = _targetGraphView.CreateDialogueNode(nodeData.dialogueText);
            tempNode.GUID = nodeData.nodeGuid;
            _targetGraphView.AddElement(tempNode);

            List<DialogueNodeLinkData> nodePorts = dialogueContainerScriptableObject.nodeLinks.Where(
                x => x.baseNodeGuid == nodeData.nodeGuid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.connectingPortName));
        }
    }

    private void ConnectNodes() {
        for (int i = 0; i < this.nodes.Count; i++) {
            List<DialogueNodeLinkData> connections = dialogueContainerScriptableObject.nodeLinks.Where(
                x => x.baseNodeGuid == this.nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++) {
                string targetNodeGuid = connections[j].targetNodeGuid;
                DialogueNode targetNode = this.nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(this.nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                DialogueNodeData firstNode = dialogueContainerScriptableObject.dialogueNodesData.First(
                    x => x.nodeGuid == targetNodeGuid);
                targetNode.SetPosition(new Rect(
                    firstNode.nodePositionOnGraph, _targetGraphView.defaultNodeSize));
            }
        }
    }

    private void LinkNodes(Port output, Port input) {
        Edge tempEdge = new Edge {
            output = output,
            input = input
        };
        
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }
}
