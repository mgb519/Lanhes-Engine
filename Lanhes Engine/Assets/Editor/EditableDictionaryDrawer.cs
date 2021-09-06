using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public abstract class EditableDictionaryDrawer<TK, TV> : PropertyDrawer {

    private bool foldout = true;
    //not 100% sure what the constant is for, other than it's used as a common dimension. Why 18 though? 
    //I'm sure vexe had his reasons
    //Yes, a lot of this code is scraped from the SerializableDictionary drawer code  at https://forum.unity.com/threads/finally-a-serializable-dictionary-for-unity-extracted-from-system-collections-generic.335797/
    private const float kButtonWidth = 18f;

    internal EditableDictionary<TK, TV> dict;
    private TK previousNewEntry;


    public override void OnGUI(Rect position, SerializedProperty dictionary, GUIContent label) {
        InitDict(dictionary);
        // Draw the GUI elements here
        EditorGUI.BeginProperty(position, label, dictionary);

        // Draw label and calculate new position
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float lineHeight = base.GetPropertyHeight(dictionary, label);

        // draw the foldout arrow
        var foldoutRect = position;
        foldoutRect.width -= 2 * kButtonWidth;
        foldoutRect.height = lineHeight;
        EditorGUI.BeginChangeCheck();
        foldout = EditorGUI.Foldout(foldoutRect, foldout, label, true);
        if (EditorGUI.EndChangeCheck()) {
            EditorPrefs.SetBool(label.text, foldout);
        }

        if (foldout) {
            // Get dictionary and draw it out

            SerializedProperty keys = dictionary.FindPropertyRelative("keys");
            SerializedProperty values = dictionary.FindPropertyRelative("values");

            for (int i = 0; i < dict.Count; i++) {
                Rect line = new Rect(position.x, position.y + lineHeight * (i + 1), position.width, lineHeight);


                Rect leftRec = new Rect(line.x, line.y, line.width * 0.45f, line.height);
                Rect rightRec = new Rect(line.x + line.width * 0.45f, line.y, line.width * 0.45f, line.height);

                //TODO: make sure we don't have duplicate keys!
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(leftRec, keys.GetArrayElementAtIndex(i), GUIContent.none);
                if (EditorGUI.EndChangeCheck()) {
                    //TODO: make sure new key is valid
                }
                EditorGUI.PropertyField(rightRec, values.GetArrayElementAtIndex(i), GUIContent.none);


                Rect removeButtonRect = new Rect(line.x + line.width * 0.9f, line.y, Math.Min(line.height, line.width * 0.1f), Math.Min(line.height, line.width * 0.1f));
                if (GUI.Button(removeButtonRect, new GUIContent("X", "Delete entry"), EditorStyles.miniButtonRight)) {
                    dict.Remove(dict.keys[i]);
                }

            }

            //draw the entry addition
            Rect bottomLine = new Rect(position.x, position.y + lineHeight * (dict.Count + 1), position.width, lineHeight);
            Rect newNameField = new Rect(bottomLine.x, bottomLine.y, bottomLine.width * 0.5f, bottomLine.height);

            Rect addButtonRect = new Rect(bottomLine.x + bottomLine.width * 0.9f, bottomLine.y, Math.Min(bottomLine.height, bottomLine.width * 0.1f), Math.Min(bottomLine.height, bottomLine.width * 0.1f));

            //TODO: don't like how we're passing the prevoius entry; this basically exists only for string dictionaries...
            TK newKey = GetNewKey(newNameField, previousNewEntry);

            previousNewEntry = newKey;
            if (GUI.Button(addButtonRect, new GUIContent("+", "Add entry"), EditorStyles.miniButtonRight)) {
                if (dict.ContainsKey(newKey)) {
                    Debug.LogWarning("Cannot add new entry: key " + newKey + " already exists");
                } else {
                    dict.Add(newKey, default(TV));
                }
            }

        }

        EditorGUI.EndProperty();

    }

    internal virtual void InitDict(SerializedProperty dictionary) {
        if (dict == null) {
            dict = fieldInfo.GetValue(dictionary.serializedObject.targetObject) as EditableDictionary<TK, TV>;
        }
    }

    internal abstract TK GetNewKey(Rect newNameField, TK previous);

    public override float GetPropertyHeight(SerializedProperty dictionary, GUIContent label) {
        // return the height of the property
        int size = GetCount(dictionary);
        return base.GetPropertyHeight(dictionary, label) * (foldout ? (size + 2) : 1);
    }

    private int GetCount(SerializedProperty dictionary) {
        InitDict(dictionary);
        int size = dict.Count;
        return size;
    }
}




public class StringIndexedDictionaryDrawer<T> : EditableDictionaryDrawer<string, T> {
    internal override string GetNewKey(Rect newNameField, string previous) {
        return EditorGUI.TextField(newNameField, previous);
    }
}

[CustomPropertyDrawer(typeof(DataManager.IntDatabase))]
public class IntDatabaseDrawer : StringIndexedDictionaryDrawer<int> { }

[CustomPropertyDrawer(typeof(DataManager.StringDatabase))]
public class StringDatabaseDrawer : StringIndexedDictionaryDrawer<string> { }

[CustomPropertyDrawer(typeof(DataManager.BoolDatabase))]
public class BoolDatabaseDrawer : StringIndexedDictionaryDrawer<bool> { }


[CustomPropertyDrawer(typeof(Inventory.InventoryContents))]
public class InventoryDrawer : EditableDictionaryDrawer<InventoryItem, int> {
    internal override InventoryItem GetNewKey(Rect newNameField, InventoryItem previous) {
        return (InventoryItem)EditorGUI.ObjectField(newNameField, previous, typeof(InventoryItem), false); //uhhhh, not 100% how good this will be...
    }


    //TODO: we shouldn't need this. this should be in the core class!
    //ew. making this function overriden is not the best way to go about this, I don't think
    //And what if we have
    internal override void InitDict(SerializedProperty dictionary) {
        if (dict == null) {

            PartyManager partyManager = dictionary.serializedObject.targetObject as PartyManager;
            //Debug.Log(dictionary.propertyPath);
            //TODO:AAAA THIS IS AWFUL 
            int idx = int.Parse(System.Text.RegularExpressions.Regex.Match(dictionary.propertyPath, @"\d+").Value);
            Debug.Log(idx);
            //Debug.Log(partyManager.parties.Length);
            Inventory holder = partyManager.parties[idx].inventory;
            dict = fieldInfo.GetValue(holder) as EditableDictionary<InventoryItem, int>;


        }
    }

}