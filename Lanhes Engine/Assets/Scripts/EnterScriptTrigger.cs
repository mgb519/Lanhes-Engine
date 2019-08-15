using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterScriptTrigger : ScriptExecutor {

    GameObject player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == player) {
            Execute();
        }
    }
}
