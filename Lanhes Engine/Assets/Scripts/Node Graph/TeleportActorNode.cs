

using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[Node(false, "Event Scripting/Teleport Actor", new Type[] { typeof(EventScriptingCanvas) })]
public class TeleportNPCNode : EventFlowNode
{
    public override string GetID { get { return "teleportActorNode"; } }


    //TODO this requires manually setting vectors, we could get editor integration y assosciating a script with a scene but...

    [SerializeField]
    public string actorName;

    [SerializeField]
    public Vector3 positionToTeleportTo;



    [NonSerialized]
    SerializedProperty posProp = null;
    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    //public override Vector2 DefaultSize { get { return new Vector2(550, 400); } }
    public override bool AutoLayout => true;



    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;





    public override void NodeGUI() {
        base.NodeGUI();

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        //scroll = EditorGUILayout.BeginScrollView(scroll);
        //EditorStyles.textField.wordWrap = true;
        EditorGUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Actor name", GUILayout.MaxWidth(80));
        actorName = EditorGUILayout.TextField(actorName, new GUILayoutOption[] { });

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (posProp== null) { posProp = thisAsSerialized.FindProperty("positionToTeleportTo"); }
        EditorGUILayout.PropertyField(posProp, false, new GUILayoutOption[] { });

        GUILayout.Space(10);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorStyles.textField.wordWrap = false;
        //EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        //TODO button to create new string
    }



    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        //NPC is teleported to location
        GameObject g = GameObject.Find(actorName);
        if (g == null) {
            Debug.LogWarning("script " + canvas.saveName + ", did not find NPC " + actorName); 
            yield return null; 
        }
        g.transform.position = positionToTeleportTo;
        //happens instantly, do not need to wait
        yield return ContinueFrom(next);
    }
}
