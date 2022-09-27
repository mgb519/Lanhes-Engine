using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;



[Node(false, "Event Scripting/Choice", new Type[] { typeof(EventScriptingCanvas) })]
public class ChoiceNode : EventFlowNode
{
    public override string GetID { get { return "choiceNode"; } }

    public override string Title { get { return "Choice Prompt"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;


    private ValueConnectionKnobAttribute dynaBranchCreationAttribute
    = new ValueConnectionKnobAttribute(
        "Next", Direction.Out, "NextEvent", NodeSide.Right);

    private ValueConnectionKnobAttribute dynaConditionCreationAttribute
    = new ValueConnectionKnobAttribute(
        "Condition", Direction.In, "Condition", NodeSide.Left);

    /*

    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;
    */

    [SerializeField]
    public LocalizedString prompt;


    [SerializeField]
    private List<Choice> _options;


    //TODO should this be a struct?
    [Serializable]
    class Choice
    {
        [SerializeField]
        public LocalizedString text;
        public bool repeats = true;
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


    //TODO reodering of options
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
            SerializedProperty choice = optionsProp.GetArrayElementAtIndex(i);

            ((ValueConnectionKnob)dynamicConnectionPorts[2 * i]).SetPosition(); //render the outgoing connection
            ((ValueConnectionKnob)dynamicConnectionPorts[2 * i + 1]).SetPosition(); //render the condition that decides if the choice is shown
            if (GUILayout.Button("-", GUILayout.Width(20))) {
                _options.RemoveAt(i);
                DeleteConnectionPort(2 * i);
                DeleteConnectionPort(2 * i + 1);
                i--;
            }

            DrawLocalizedStringProperty(choice.FindPropertyRelative("text"), ref option.text);

            //TODO place a label for the checkbox
            option.repeats= EditorGUILayout.Toggle(option.repeats,new GUILayoutOption[] { });
          
            //option.OptionDisplay = EditorGUILayout.TextArea(option.OptionDisplay, GUILayout.MinWidth(80));
           

            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            //EditorGUILayout.Space();
        }

        EditorGUILayout.EndVertical();

        thisAsSerialized.ApplyModifiedProperties();
      
    }

    private void AddNewOption() {
        Choice option = new Choice();
        thisAsSerialized = null;
        optionsProp = null;

        CreateValueConnectionKnob(dynaBranchCreationAttribute);
        CreateValueConnectionKnob(dynaConditionCreationAttribute);

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



    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        //we have a choice window
        //TODO: ths currently only works for string selection, as that is how Ink worked. Figure out a method to allow for other slection types..
        //TODO filter choices with conditions for appearance
        List<Choice> choices = _options;
        int idx = 0;
        List<Choice> filter = new List<Choice>();
        foreach (Choice c in choices) {
            //if no special condition attached, or the condition evaluates to true, we can select
                if (dynamicConnectionPorts[2 * idx + 1].connections.Count == 0 || ((ConditionNode)(dynamicConnectionPorts[2 * idx + 1].connections[0].body)).Evaluate(canvasData)) {
                    if (c.repeats || !canvasData.ContainsKey((this, idx.ToString()))) {
                        filter.Add(c);
                    }
                
            }
            idx++;
        }
        List<LocalizedString> choicesAsStrings = new List<LocalizedString>();
        foreach (Choice c in filter) { choicesAsStrings.Add(c.text); }

        SelectionWindow s = WindowManager.CreateStringSelection(choicesAsStrings, null, prompt); 
        yield return new WaitUntil(() => WindowManager.ContinuePlay());
        LocalizedString selected = ((SelectableString)(s.selected)).data;

        //TODO: sure this could be optimised
        int index = _options.FindIndex(x => x.text == selected);
        Debug.Log(_options[index].repeats);
        //If we chose a dialogue that shouldn't be repeated, grey it out
        if (!_options[index].repeats) {
            canvasData.Add((this, index.ToString()),1);
        }

        


        yield return ContinueFrom(dynamicConnectionPorts[2*index]);
    }


}
