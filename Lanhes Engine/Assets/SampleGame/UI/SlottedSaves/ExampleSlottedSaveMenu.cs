using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExampleSlottedSaveMenu : SaveMenu
{
    private bool isSaving = true;

    public override void DeleteSave(string path) {
        throw new System.NotImplementedException();
    }


    //TODO this should not exist, get this statically
    public InventoryItem goldItem;


    //TODO some sign that we are in save rather then load mode.
    public override void LoadMode() {
        isSaving = false;
    }

    public override void SaveMode() {
        isSaving = true;
    }

    internal override void SaveHeader(ref StreamWriter file) {
        //TODO shouldn't we be getting the data through the XML document maybe, rather than direct data access?  
        int gold = PartyManager.GetParty().inventory.HowManyOfItem(goldItem);//TODO get Gold statically
        file.WriteLine("Money: " + gold.ToString());
    }

    internal override void SkipHeader(ref StreamReader file) {
        file.ReadLine();//skip the gold number
    }

    public void SaveSelected(string path) {
        if (isSaving) {
            //save over the selected slot

            //TODO confirmation dialog
            SaveGame(path);
        } else {
            //load the selected slot

            //TODO confirmation dialogue
            LoadGame(path);
            CollapseMenu();
        }
    }

}
