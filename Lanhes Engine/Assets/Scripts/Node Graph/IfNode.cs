using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using System;




[Node(false, "Event Scripting/Branch", new Type[] { typeof(EventScriptingCanvas) })]
public class IfNode : EventFlowNode
{
    public override string GetID { get { return "ifNode"; } }

    [ValueConnectionKnob("Condition", Direction.In, "Condition", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob cond;


    [ValueConnectionKnob("Next(False)", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob nextIfFalse;


    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        bool c = true;
        if (cond.connections.Count != 0) {
            c = ((ConditionNode)cond.body).Evaluate(canvasData);
        }

        yield return ContinueFrom(c ? nextIfFalse : nextIfFalse);
    }
}
