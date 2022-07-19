using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NodeEditorFramework;
using UnityEditor;



[Node(false, "Event Scripting/Event Node")]
public abstract class EventNode : Node
{
    public abstract IEnumerator Execute();

    public EventNode ContinueFrom(ValueConnectionKnob v) {
        if (v.connections.Count == 0) { return null; } else { return (EventNode)v.connection(0).body; }
    }
}


public class NextEventType : ValueConnectionType // : IConnectionTypeDeclaration
{
    public override string Identifier { get { return "NextEvent"; } }
    public override Type Type { get { return typeof(EventNode); } }
    public override Color Color { get { return Color.cyan; } }
}