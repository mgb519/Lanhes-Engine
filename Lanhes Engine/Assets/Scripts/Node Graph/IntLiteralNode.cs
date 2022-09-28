using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Node(false, "Event Scripting/Math/Integer Literal", new Type[] { typeof(EventScriptingCanvas) })]
public class IntLiteralNode : IntNode
{
    public override string GetID { get { return "IntLiteralNode"; } }

    public override string Title { get { return "Integer Literal"; } }

    public override Vector2 MinSize { get { return new Vector2(100, 100); } }
    public override bool AutoLayout => true;

    [SerializeField]
    public int value;

    public override int Evaluate(Dictionary<(EventNode, string), int> canvasData) {
        return value;
    }


    public override void NodeGUI() {
        base.NodeGUI();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(80));
        value = EditorGUILayout.IntField(value, new GUILayoutOption[] { });

        EditorGUILayout.EndHorizontal();
    }
}
