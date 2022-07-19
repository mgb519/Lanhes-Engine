using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using NodeEditorFramework;
[Node(false, "Event Scripting/Event Starting node", new Type[] { typeof(EventScriptingCanvas) })]
public class EventStartNode : EventNode
{
    public static string ID = "StartNode";
    public override string GetID {get{return ID;}}

    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30,MaxConnectionCount =ConnectionCount.Single)]
    public ValueConnectionKnob next;

    public override IEnumerator Execute() {
        yield return ContinueFrom(next);
    }

    public override void NodeGUI() {
        //name = TextField(name);

        foreach (ConnectionKnob knob in connectionKnobs) {
            knob.DisplayLayout();
        }
    }
}
