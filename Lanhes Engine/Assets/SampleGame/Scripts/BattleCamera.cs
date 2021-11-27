using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    
    private void Awake() {
        PartyManager.playerInThisScene.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        PartyManager.playerInThisScene.gameObject.SetActive(true);
    }
}
