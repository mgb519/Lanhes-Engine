using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[Node(false, "Event Scripting/Get Boolean Variable", new Type[] { typeof(EventScriptingCanvas) })]
public class GetBool : ConditionNode
{


    public override string GetID { get { return "getBoolNode"; } }

    public override string Title { get { return "Get Boolean Variable"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;

    [SerializeField]
    string varaibleName;
    public override bool Evaluate(Dictionary<(EventNode, string), int> canvasData) {

        return DataManager.GetBool(varaibleName);
    }
    public override void NodeGUI() {
        base.NodeGUI();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Variable Name", GUILayout.MaxWidth(80));
        varaibleName = EditorGUILayout.TextField(varaibleName, new GUILayoutOption[] { });

        EditorGUILayout.EndHorizontal();
    }
}
