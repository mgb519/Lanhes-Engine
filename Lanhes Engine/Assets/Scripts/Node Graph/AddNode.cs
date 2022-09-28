using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[Node(false, "Event Scripting/Math/Add Integers", new Type[] { typeof(EventScriptingCanvas) })]
public class AddNode : IntNode
{


    public override string GetID { get { return "addIntNode"; } }

    public override string Title { get { return "+"; } }

    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;



    [ValueConnectionKnob("Left", Direction.In, "Integer", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob left;
    [ValueConnectionKnob("Right", Direction.In, "Integer", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob right;


    public override int Evaluate(Dictionary<(EventNode, string), int> canvasData) {
        return ((IntNode)(left.connections[0].body)).Evaluate(canvasData) + ((IntNode)(right.connections[0].body)).Evaluate(canvasData);
    }
 


}
