using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ShopEntryButton : MonoBehaviour {


    //public GameObject itemPanelTemplate;
    private Button button;
    private GameObject itemPanel;
    private ItemCost data;

    // Start is called before the first frame update
    void Awake() {
        button = gameObject.GetComponent<Button>();
    }

    public void SetData(ItemCost data) {
        this.data = data;
        button.GetComponentInChildren<Text>().text = data.item.name;

    }


    //The panel-related functions are game-specific
    public virtual void MouseOver() {
        //Debug.Log("Making item panel");
        //create a popup menu of the cost of this item
        //TODO: show stats of item? maybe game-specific
        //itemPanel = Instantiate(itemPanelTemplate);
        //itemPanel.transform.SetParent(transform.parent);
    }

    public virtual void MouseExit() {
        //Destroy(itemPanel);
    }

    public virtual void Clicked() {
        SendMessageUpwards("Transaction", data);
    }

}
