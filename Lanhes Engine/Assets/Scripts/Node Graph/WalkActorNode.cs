using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



[Node(false, "Event Scripting/Actor Walk Sequence", new Type[] { typeof(EventScriptingCanvas) })]
public class WalkActorNode : EventFlowNode
{
    public override string GetID { get { return "walkActorNode"; } }


    [Serializable]
    public struct Sequence {
        public string actorName;
        public List<Vector3> waypoints;
    }


    [SerializeField]
    public List<Sequence> actorsAndWaypointSequences = new List<Sequence>();
    //public string actorName;
    //public Vector3[] waypointSequence;


    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    public override bool AutoLayout => true;
    [NonSerialized]
    SerializedProperty aawsProp;

    
    private HashSet<PawnMovementController> overridenNPCs = new HashSet<PawnMovementController>();


    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;



    protected override void OnCreate() {
        base.OnCreate();
        actorsAndWaypointSequences = new List<Sequence>();
        overridenNPCs = new HashSet<PawnMovementController>();
    }

    public override void NodeGUI() {
        base.NodeGUI();
        
        if (aawsProp == null) {
            aawsProp = thisAsSerialized.FindProperty(nameof(actorsAndWaypointSequences));
        }


        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(aawsProp, new GUILayoutOption[] { });

        EditorGUILayout.EndVertical();

    }
    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        foreach (Sequence x in actorsAndWaypointSequences) {
            //NPC walks to positon

            string actorName = x.actorName;
            List<Vector3> waypointSequence = x.waypoints;

            //TODO what if some other event triggers while the NPC is walking? This leaves that option open...

            GameObject g = GameObject.Find(actorName);
            if (g == null) { Debug.LogWarning("script " + canvas.saveName + ", did not find NPC " + actorName); yield return null; }
            PawnMovementController controller = g.GetComponent<PawnMovementController>();
            if (g == null) { Debug.LogWarning("script " + canvas.saveName + ", NPC " + actorName + " can't be directed"); yield return null; }

            overridenNPCs.Add(controller);

            foreach (Vector3 v in waypointSequence) {
                Debug.Log("told " + actorName + "to move to " + v.ToString());
                controller.AddWaypoint(v);
            }
        }

        //TODO: getting the Y ever so slightly wrong can result in this never being triggered, as the agent cannot actually move freely on Y.
        yield return new WaitUntil(() => overridenNPCs.All(x => x.ClearedPath()));
        foreach (PawnMovementController controller in overridenNPCs) {
            controller.Release();  //TODO presumably, we may want the NPC to stay in pace. maybe then we shouldn't free the waypoint, in case its normal AI takes hold? In this case, the waypoint needs to be freed up at *some* point. Except for cases where we alter patrol paths?
        }
        overridenNPCs.Clear();

        yield return ContinueFrom(next);
    }
}
