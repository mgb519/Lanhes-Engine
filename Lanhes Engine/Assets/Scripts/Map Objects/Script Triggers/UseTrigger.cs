using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UseTrigger : Trigger
{
    public void Used() {
        scriptToCall.Action();
    }

}
