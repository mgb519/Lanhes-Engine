using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using UnityEngine.Localization;
using UnityEditor;
using System;


[Node(false, "Event Scripting/Display String Window", new Type[] { typeof(EventScriptingCanvas) })]
public class DisplayStringNode : EventFlowNode
{
    public override string GetID { get {return "displayStringNode";}}

	[SerializeField]
    public LocalizedString message;

	public override Vector2 MinSize { get { return new Vector2(400, 100); } }
	//public override Vector2 DefaultSize { get { return new Vector2(550, 400); } }
	public override bool AutoLayout => true;


	[ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
	public ValueConnectionKnob next;


    [NonSerialized]
    SerializedProperty msgAsSerialized = null;

	public override void NodeGUI() {
        base.NodeGUI();

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        //scroll = EditorGUILayout.BeginScrollView(scroll);
        //EditorStyles.textField.wordWrap = true;
        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(10);
        if (msgAsSerialized == null) {
            msgAsSerialized = thisAsSerialized.FindProperty("message");
        }
        DrawLocalizedStringProperty(msgAsSerialized,ref message);
        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();
        EditorStyles.textField.wordWrap = false;
        //EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        //TODO button to create new string
    }


    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
		//TODO: get name and picture, etc
		WindowManager.CreateStringWindow(message, null);
		yield return new WaitUntil(() => WindowManager.ContinuePlay());
		yield return ContinueFrom(next);
    }
}
