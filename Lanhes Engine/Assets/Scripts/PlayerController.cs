using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Inventory inventory;

    //TODO: for game testing remove this later
    public InertItem currency;

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < 1000; i++) {
            inventory.AddItem(currency);
        }
    }

    // Update is called once per frame
    void Update() {
        

        if (Input.GetButtonDown("Pause") && Time.timeScale != 0) {
            WindowManager.CreatePauseWindow();          

        }

    }
}
