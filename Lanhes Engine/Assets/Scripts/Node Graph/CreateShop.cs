using NodeEditorFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Node(false, "Event Scripting/Shop", new Type[] { typeof(EventScriptingCanvas) })]
public class CreateShop : EventFlowNode
{
    public override Vector2 MinSize { get { return new Vector2(400, 100); } }
    //public override Vector2 DefaultSize { get { return new Vector2(550, 400); } }
    public override bool AutoLayout => true;

    public override string GetID { get { return "shopNode"; } }

    [ValueConnectionKnob("Next", Direction.Out, "NextEvent", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Single)]
    public ValueConnectionKnob next;

    [SerializeField]
    public ShopData shop;

    [NonSerialized]
    SerializedProperty shopProp;

    public override IEnumerator Execute(Dictionary<(EventNode, string), int> canvasData) {
        WindowManager.CreateShopWindow(shop.buyPrices, shop.sellPrices, PartyManager.GetParty().inventory, null);
        yield return new WaitUntil(() => WindowManager.ContinuePlay());
        yield return ContinueFrom(next);
    }



    public override void NodeGUI() {
        base.NodeGUI();

        if (shopProp == null) {
            shopProp = thisAsSerialized.FindProperty(nameof(shop));
        }


        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(shopProp, new GUILayoutOption[] { });

        EditorGUILayout.EndVertical();

    }
}
