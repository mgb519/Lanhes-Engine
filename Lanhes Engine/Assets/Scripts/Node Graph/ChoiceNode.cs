using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;



//TODO implment "allow choice to only be selected once" feature link Ink had. This will be saved in GraphEvent I suppose
[Node(false, "Event Scripting/Choice", new Type[] { typeof(EventScriptingCanvas) })]
public class ChoiceNode : EventFlowNode
{
    public override string GetID { get { return "choiceNode"; } }

    public override string Title { get { return "Choice Prompt"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;


    private ValueConnectionKnobAttribute dynaCreationAttribute
    = new ValueConnectionKnobAttribute(
        "Next", Direction.Out, "NextEvent", NodeSide.Right);
    /*

    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;
    */

    [SerializeField]
    public LocalizedString prompt;


    [SerializeField]
    private List<Choice> _options;

    [Serializable]
    class Choice
    {
        [SerializeField]
        public LocalizedString text;
        //public int NodeOutputIndex;


    }
    /*
    [CustomPropertyDrawer(typeof(Choice), true)]
    class ChoicePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //base.OnGUI(position, property, label);

            //EditorGUILayout.BeginVertical();
            //EventNode.DrawLocalizedStringProperty(property.FindPropertyRelative("text"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("text"),false,new GUILayoutOption[] { });
            //EditorGUILayout.EndVertical();
        }
    }*/


    [NonSerialized]
    SerializedProperty promptAsSerialized = null;

    [NonSerialized]
    SerializedProperty optionsProp = null;

    protected override void OnCreate() {
        base.OnCreate();
        _options = new List<Choice>();

        AddNewOption();
    }

    public override void NodeGUI() {
        base.NodeGUI();
        //EditorGUILayout.BeginHorizontal();

        if (promptAsSerialized == null) {
            promptAsSerialized = thisAsSerialized.FindProperty("prompt");
        }
        DrawLocalizedStringProperty(promptAsSerialized, ref prompt);


        EditorGUILayout.Space(5);
        DrawOptions();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space(5);
        if (GUILayout.Button("Add New Option")) {
            AddNewOption();
            IssueEditorCallBacks();
        }

        EditorGUILayout.EndVertical();
        //EditorGUILayout.EndHorizontal();
    }


    //TODO reodering of options, and allow removing arbitrary options
    private void DrawOptions() {

        EditorGUILayout.BeginVertical();
        if (optionsProp == null) {
            optionsProp = thisAsSerialized.FindProperty("_options");
        }
        for (var i = 0; i < _options.Count; i++) {
            Choice option = _options[i];
            EditorGUILayout.BeginVertical();
            //EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i + ".", GUILayout.MaxWidth(15));
            DrawLocalizedStringProperty(optionsProp.GetArrayElementAtIndex(i).FindPropertyRelative("text"),ref _options[i].text);
            //option.OptionDisplay = EditorGUILayout.TextArea(option.OptionDisplay, GUILayout.MinWidth(80));
            ((ValueConnectionKnob)dynamicConnectionPorts[i]).SetPosition();
            if (GUILayout.Button("-", GUILayout.Width(20))) {
                _options.RemoveAt(i);
                DeleteConnectionPort(i);
                i--;
            }

            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            //EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();


        /*
        int i = 0;
        Debug.Log(_options.Count);
        foreach (Choice c in _options) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            Debug.Log(c);
            //TODO this is incorrect!
            EditorGUILayout.PropertyField(new UnityEditor.SerializedObject(c).FindProperty("text"), false, new GUILayoutOption[] { });
            ((ConnectionKnob)dynamicConnectionPorts[i++]).SetPosition();

            EditorGUILayout.Space(5);
            if (GUILayout.Button("Remove Choice")) {
                RemoveChoice(c);
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }*/
    }

    private void AddNewOption() {
        Choice option = new Choice();
        thisAsSerialized = null;
        optionsProp = null;

        CreateValueConnectionKnob(dynaCreationAttribute);

        //option.NodeOutputIndex = dynamicConnectionPorts.Count - 1;
        _options.Add(option);
        //RebuildConnectors();
    }

    /*
     private void RemoveChoice(Choice choice) {
         int removedIndex = choice.NodeOutputIndex;
         //Everything above this index must have its index decremented, and and the connections moved over.

         for (int i = removedIndex; i < _options.Count; i++) {
             dynamicConnectionPorts[_options[i].NodeOutputIndex].body = dynamicConnectionPorts[_options[i-1].NodeOutputIndex].body;
             _options[i].NodeOutputIndex--;
         }


         DeleteConnectionPort(removedIndex);
         _options.RemoveAt(removedIndex);

     }*/
    private void RebuildConnectors() {
        throw new NotImplementedException();
    }


    //For Resolving the Type Mismatch Issue that apparently exist according to https://github.com/Seneral/Node_Editor_Framework/blob/Examples/Dialogue-System/ExampleDialogSystem/Core/Nodes/MultiPathSelectorNode.cs
    private void IssueEditorCallBacks() {
        NodeEditorCallbacks.IssueOnAddConnectionPort(dynamicConnectionPorts[_options.Count - 1]);

        //TODO uncomment if something happens
    }



    public override IEnumerator Execute() {
        //we have a choice window
        //TODO: ths currently only works for string selection, as that is how Ink worked. Figure out a method to allow for other slection types..
        List<Choice> choices = _options;
        List<LocalizedString> choicesAsStrings = new List<LocalizedString>();
        foreach (Choice c in choices) { choicesAsStrings.Add(c.text); }

        SelectionWindow s = WindowManager.CreateStringSelection(choicesAsStrings, null, prompt); //TODO some way of getting a prompt from Ink
        yield return new WaitUntil(() => WindowManager.ContinuePlay());
        LocalizedString selected = ((SelectableString)(s.selected)).data;
        //TODO: sure this could be optimised
        int index = _options.FindIndex(x => x.text == selected);
        ContinueFrom(dynamicConnectionPorts[index]);
    }


}
