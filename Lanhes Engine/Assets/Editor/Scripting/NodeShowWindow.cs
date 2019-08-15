using System;
using UnityEditor;
using UnityEngine;


public class NodeShowWindow : ScriptNode {
    const string BASETITLE = "Display Text Window";
    const int MAXSTRINGDISPLAYSIZE = 55;
    string text = "";

    public NodeShowWindow(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<ScriptNode> OnClickRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode) {
        //outPoints.Add("", new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint));
        title = BASETITLE;
    }

    public override void Draw() {
        title = BASETITLE + "\n" + text;
        //trim title to size
        if (title.Length >= MAXSTRINGDISPLAYSIZE) { title = title.Remove(MAXSTRINGDISPLAYSIZE - 5); title += "..."; }
        base.Draw();
        if (isSelected) {
            text = EditorGUILayout.TextField("Text:", text);
        }




    }

}
