using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow {
    private DialogueGraphView graphView;
    private string fileName = "New narrative";
    
    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow() {
        DialogueGraph window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable() {
        ConstructGraphView();
        GenerateToolBar();
    }

    private void OnDisable() {
        this.rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView() {
        graphView = new DialogueGraphView();
        graphView.name = "Dialogue Graph";
        graphView.StretchToParentSize();
        this.rootVisualElement.Add(graphView);
    }

    private void GenerateToolBar() {
        Toolbar toolbar = new Toolbar();

        // this textField names the dialogue tree for saving
        TextField fileNameTextField = new TextField("File name:");
        // setting default file name
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        // if the user wants to change the file name update it
        fileNameTextField.RegisterValueChangedCallback(evt => {
            this.fileName = evt.newValue;
        });  
        toolbar.Add(fileNameTextField);

        Button saveDataButton = new Button(clickEvent: () => {
            RequestDataOpertaion(true);
        });
        saveDataButton.text = "Save Data";
        toolbar.Add(saveDataButton);

        Button loadDataButton = new Button(clickEvent: () => {
            RequestDataOpertaion(false);
        });
        loadDataButton.text = "Load Data";
        toolbar.Add(loadDataButton);
        
        Button createNodeButton = new Button(clickEvent: () => {
            graphView.CreateNode("Dialogue node");
        });
        createNodeButton.text = "Create Node";
        toolbar.Add(createNodeButton);
        
        this.rootVisualElement.Add(toolbar);
    }

    private void RequestDataOpertaion(bool requestToSave) {
        if (string.IsNullOrEmpty(this.fileName)) {
            EditorUtility.DisplayDialog(
                "Invalid file name.",
                "Enter a valid file name (field may have been left empty).",
                "OK");
        }
        
        GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(this.graphView);

        if (requestToSave) {
            saveUtility.SaveGraph(this.fileName);
        }
        else {
            saveUtility.LoadGraph(this.fileName);
        }
    }
}