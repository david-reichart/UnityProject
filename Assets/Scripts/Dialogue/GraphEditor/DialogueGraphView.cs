using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView {
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    
    public DialogueGraphView() {
        this.styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraphStyle"));
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        GridBackground background = new GridBackground();
        this.Insert(0, background);
        background.StretchToParentSize();
        this.AddElement(GenerateEntryPointNode());
    }

    private DialogueNode GenerateEntryPointNode() {
        // creating a new dialogue node
        DialogueNode dialogueNode = new DialogueNode();
        dialogueNode.title = "Start";
        dialogueNode.GUID = Guid.NewGuid().ToString(); // unique id
        dialogueNode.dialogueText = "EntryPoint";
        dialogueNode.isEntryPoint = true;
        dialogueNode.SetPosition(new Rect(100, 200, 100, 150));
        // adding ports to a node
        Port nodePort = GenerateNodePort(dialogueNode, Direction.Output, Port.Capacity.Single);
        nodePort.portName = "Next";
        dialogueNode.outputContainer.Add(nodePort);
        // refresh is called after adding ports to a node
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        
        return dialogueNode;
    }

    private Port GenerateNodePort(DialogueNode dialogueNode, Direction inOrOut, Port.Capacity portCapacity) {
        return dialogueNode.InstantiatePort(Orientation.Horizontal, inOrOut, portCapacity, typeof(int));
    }

    public void CreateNode(string nodeName) {
        this.AddElement(CreateDialogueNode(nodeName));
    }

    public DialogueNode CreateDialogueNode(string nodeName) {
        DialogueNode dialogueNode = new DialogueNode();
        dialogueNode.title = nodeName;
        dialogueNode.dialogueText = nodeName;
        dialogueNode.GUID = Guid.NewGuid().ToString();

        Port inputPort = GenerateNodePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        Button button = new Button(() => {
            AddChoicePort(dialogueNode);
        });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        TextField dialogueTextField = new TextField(string.Empty);
        dialogueTextField.RegisterValueChangedCallback(evt => {
            dialogueNode.dialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        dialogueTextField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(dialogueTextField);
        
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return dialogueNode;
    }

    // This override method allows nodes to connect to each other regardless of in/out data type
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
        List<Port> compatablePorts = new List<Port>();
        ports.ForEach((port) => {
            // a port cannot connect to itself
            // the input port of a node cannot be connected to its own output
            if (startPort != port && startPort.node != port.node) {
                compatablePorts.Add(port); // add any port that meets these rules
            }
        });

        return compatablePorts;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "") {
        Port generatedPort = GenerateNodePort(dialogueNode, Direction.Output, Port.Capacity.Single);
        int outputPortsOnThisNode = dialogueNode.outputContainer.Query("connector").ToList().Count;

        // if given a port name when this is called, apply it
        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortsOnThisNode + 1}"
            : overriddenPortName;

        TextField textField = new TextField {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        textField.style.minWidth = 60;
        textField.style.maxWidth = 100;
        generatedPort.contentContainer.Add(new Label(" "));
        generatedPort.contentContainer.Add(textField);
        
        Button deleteButton = new Button(() => {
            RemovePort(dialogueNode, generatedPort);
        });
        deleteButton.text = "x";
        generatedPort.contentContainer.Add(deleteButton);
        
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort) {
        var targetEdge = edges.ToList().Where(
            x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any()) {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }
        
        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
