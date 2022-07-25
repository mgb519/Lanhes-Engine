using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;


[NodeCanvasType("Event Scripting Canvas")]
public class EventScriptingCanvas : NodeCanvas
{
    public override string canvasName { get { return "Event"; } }
    public string Name = "Event";
    public override bool allowRecursion { get { return true; } }

    public EventStartNode rootNode;
    

    protected override void OnCreate() {
        //Debug.Log("creating...");
        //Traversal = new GraphTraversal(this);
        //Debug.Log(Node.Create(EventStartNode.ID, Vector2.zero, this, null, true,true));
        rootNode = Node.Create(EventStartNode.ID, Vector2.zero, this, null, true) as EventStartNode;
        //Debug.Log(rootNode);
    }

    protected override void ValidateSelf() {
        //if (Traversal == null)
        //    Traversal = new GraphTraversal(this);
        //Debug.Log("validating...");
        if (!nodes.Exists((Node n) => n.GetID == EventStartNode.ID)) {
            Debug.Log("Missing a start node, creating...");
            rootNode = Node.Create(EventStartNode.ID, Vector2.zero, this, null, true) as EventStartNode;
        } else if (rootNode == null) { 
            rootNode= (EventStartNode)nodes.Find((Node n) => n.GetID == EventStartNode.ID);
        }
    }

    public override bool CanAddNode(string nodeID) {
        //Debug.Log ("Check can add node " + nodeID);
        if (nodeID == EventStartNode.ID) {
            //Debug.Log("Someone is trying to add a Start Node!");
            return !nodes.Exists((Node n) => n.GetID == EventStartNode.ID);
        }
        return true;
    }

}
