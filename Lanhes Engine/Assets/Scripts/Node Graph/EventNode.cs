using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEditor.Localization;

[Node(false, "Event Scripting/Event Node")]
public abstract class EventNode : Node
{
    [NonSerialized]
    internal SerializedObject thisAsSerialized = null;
    public abstract IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData);

    public EventNode ContinueFrom(ConnectionPort v) {
        if (v.connections.Count == 0) { return null; } else { return (EventNode)v.connection(0).body; }
    }

    public override void NodeGUI() {
        base.NodeGUI();

        if (thisAsSerialized == null) {
            thisAsSerialized = new UnityEditor.SerializedObject(this);
        }
    }


    internal void DrawLocalizedStringProperty(SerializedProperty prop, ref LocalizedString stringItself) {
        //EditorGUILayout.BeginHorizontal();
        if (!stringItself.IsEmpty) {
            //EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(prop, false, new GUILayoutOption[] { });

            //TODO slightly more compact and easy-to-read view of the text that will actuall be displayed...
            //EditorGUILayout.LabelField("\""+stringItself.GetLocalizedString()+"\"", new GUILayoutOption[] { });
            //Debug.Log("test:" + stringItself.GetLocalizedString());
        } else {
            if (GUILayout.Button("Generate Unique")) {
                StringTableCollection tableCollection =
                LocalizationEditorSettings.GetStringTableCollection(LocalizationSettings.StringDatabase.DefaultTable);

                string nodeName = this.GetID;
                string pathToProp;
                int idx = 0;
                do {
                    pathToProp = canvas.savePath + "/" + nodeName + idx.ToString() + "/" + prop.propertyPath;
                    idx++;
                } while (tableCollection.SharedData.GetEntry(pathToProp) != null);

                //Debug.Log(tableCollection);
                var r = tableCollection.SharedData.AddKey(pathToProp);
                stringItself.TableReference = LocalizationSettings.StringDatabase.DefaultTable;
                stringItself.TableEntryReference = r.Key;




                EditorUtility.SetDirty(tableCollection);
                EditorUtility.SetDirty(tableCollection.SharedData);
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(canvas);
                //EditorUtility.SetDirty(prop.objectReferenceValue);
                //EditorUtility.SetDirty(prop.exposedReferenceValue);
                canvas.Validate();
                NodeEditorCallbacks.IssueOnLoadCanvas(canvas);
                canvas.TraverseAll();
                NodeEditor.RepaintClients();


            }
        }
        //EditorGUILayout.EndHorizontal();

    }
}


public abstract class EventFlowNode : EventNode
{

    [ValueConnectionKnob("Previous", Direction.In, "NextEvent", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Multi)]
    public ValueConnectionKnob prev;
}

public class NextEventType : ValueConnectionType // : IConnectionTypeDeclaration
{
    public override string Identifier { get { return "NextEvent"; } }
    public override Type Type { get { return typeof(EventNode); } }
    public override Color Color { get { return Color.cyan; } }
}





public abstract class ConditionNode : Node
{

    [ValueConnectionKnob("Output", Direction.Out, "Condition", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob output;

    [NonSerialized]
    internal SerializedObject thisAsSerialized = null;
    public abstract bool Evaluate(Dictionary<(EventNode, string), int> canvasData);

    public override void NodeGUI() {
        base.NodeGUI();
    }

}


public class ConditionType : ValueConnectionType // : IConnectionTypeDeclaration
{
    public override string Identifier { get { return "Condition"; } }
    public override Type Type { get { return typeof(bool); } }
    public override Color Color { get { return Color.green; } }
}




public abstract class IntNode : Node
{

    [ValueConnectionKnob("Output", Direction.Out, "Integer", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob output;

    [NonSerialized]
    internal SerializedObject thisAsSerialized = null;
    public abstract int Evaluate(Dictionary<(EventNode, string), int> canvasData);

    public override void NodeGUI() {
        base.NodeGUI();
    }

}


public class IntTransferType : ValueConnectionType // : IConnectionTypeDeclaration
{
    public override string Identifier { get { return "Integer"; } }
    public override Type Type { get { return typeof(int); } }
    public override Color Color { get { return Color.red; } }
}