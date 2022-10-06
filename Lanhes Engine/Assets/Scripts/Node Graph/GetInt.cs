using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[Node(false, "Event Scripting/Math/Get Integer Variable", new Type[] { typeof(EventScriptingCanvas) })]
public class GetInt : IntNode
{


    public override string GetID { get { return "getIntNode"; } }

    public override string Title { get { return "Get Integer Variable"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;

    [SerializeField]
    public string varaibleName;
    public override int Evaluate(Dictionary<(EventNode, string), int> canvasData) {

        return DataManager.GetInt(varaibleName);
    }



    public override void NodeGUI() {
        base.NodeGUI();



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Variable Name", GUILayout.MaxWidth(80));
        varaibleName = EditorGUILayout.TextField(varaibleName, new GUILayoutOption[] { });

        EditorGUILayout.EndHorizontal();
    }
}
