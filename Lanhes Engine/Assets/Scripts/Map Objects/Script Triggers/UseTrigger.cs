using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UseTrigger : Trigger
{

    [SerializeField]
    private string labelString;

    private bool mousedOver = false;
    private bool displayLabel = false;
    public void Used() {
        scriptToCall.Action();
    }

    internal void RequestLabel() {
        mousedOver = true;
        displayLabel = true;
        WindowManager.CreateUsableLabel(this);
    }

    public void Update() {
        if (!mousedOver) { displayLabel = false; }
        mousedOver = false;
    }
    public string GetLabelName() {
        return labelString;
    }
    public bool DisplayLabel() { return displayLabel; }
}
