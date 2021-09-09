using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSaveSlot : MonoBehaviour
{
    [SerializeField]
    private ExampleEmptySaveSlot emptySlot;

    [SerializeField]
    private ExampleFullSaveSlot fullSlot;

    [SerializeField]
    private string path;


    public void Awake() {
        Refresh();
    }
    public void Refresh() {
        Destroy(transform.GetChild(0));
        if (System.IO.File.Exists(path)) {
            ExampleFullSaveSlot f = GameObject.Instantiate(fullSlot,this.transform);
            f.SetPath(path);
        } else {
            ExampleEmptySaveSlot e = GameObject.Instantiate(emptySlot,this.transform);
        }
    
    }


    public void Clicked() {
        SendMessageUpwards("SaveSelected", path);
        Refresh();
    }


}
