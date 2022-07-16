using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using UnityEngine.Localization;
using UnityEditor;

[Node(false, "Event Scripting/Display String Window")]
public class DisplayStringNode : EventNode
{
    public override string GetID { get {return "displayStringNode";}}

	[SerializeField]
    public LocalizedString message;

	public override Vector2 MinSize { get { return new Vector2(350, 200); } }
	//public override Vector2 DefaultSize { get { return new Vector2(550, 400); } }
	public override bool AutoLayout => true;

    private Vector2 scroll;
	public override void NodeGUI() {

		GUILayout.BeginVertical();

		GUILayout.Space(10);
		//scroll = EditorGUILayout.BeginScrollView(scroll);
		//EditorStyles.textField.wordWrap = true;
		EditorGUILayout.BeginHorizontal();

		GUILayout.Space(10);
		EditorGUILayout.PropertyField(new UnityEditor.SerializedObject(this).FindProperty("message"), false,new GUILayoutOption[] { });//EditorGUILayout.TextArea(DialogLine, GUILayout.ExpandHeight(true));

		GUILayout.Space(10);
		EditorGUILayout.EndHorizontal();
		EditorStyles.textField.wordWrap = false;
		//EditorGUILayout.EndScrollView();
		GUILayout.Space(10);
		GUILayout.EndVertical();

	}


}
