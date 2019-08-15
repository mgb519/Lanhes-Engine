using System;
using UnityEditor;
using UnityEngine;

public class NodeTerminate : ScriptNode {
    public NodeTerminate(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<ScriptNode> OnClickRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode) {
        title = "TERMINATE SCRIPT";
    }


    public override void Draw() {
        inPoint.Draw();
        //no out-point, since it's aterminator
        //TOOD: when you write for multiple exit points, check back if this override is still desirable
        //said code may just support an empty out-node list
        GUI.Box(rect, title, style);
    }
}
