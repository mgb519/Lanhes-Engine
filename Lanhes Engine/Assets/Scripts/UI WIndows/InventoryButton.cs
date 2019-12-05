using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour {
    //TODO: all the X-Button classes coult inherit from a common class since they all have similair fields and methods... i.e this could be an extended SelectionButton<InventoryItem>
    private Button button;
    private InventoryItem data;

    void Awake() {
        button = gameObject.GetComponent<Button>();
    }


    public void SetData(InventoryItem data) {
        this.data = data;
        //TODO: factor this line out into an overridable function so the game can override how it displays items
        button.GetComponentInChildren<Text>().text = data.name;

    }

    public void Clicked() {
        SendMessageUpwards("ItemClicked", data);
    }

}
