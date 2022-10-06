using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using System;
using UnityEditor;

[Node(false, "Event Scripting/Set Integer Variable", new Type[] { typeof(EventScriptingCanvas) })]
public class SetInt : EventFlowNode
{
    public override string GetID { get { return "setIntNode"; } }

    public override string Title { get { return "Set Integer Variable"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;


    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;



    [ValueConnectionKnob("Value", Direction.In, "Integer", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob value;

    [SerializeField]
    public string variableName;

    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        int val = ((IntNode)value.connections[0].body).Evaluate(canvasData);
        DataManager.SetInt(variableName, val);


        yield return ContinueFrom(next);
    }


    public override void NodeGUI() {
        base.NodeGUI();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Variable Name", GUILayout.MaxWidth(80));
        variableName = EditorGUILayout.TextField(variableName, new GUILayoutOption[] { });

        EditorGUILayout.EndHorizontal();
    }
}
