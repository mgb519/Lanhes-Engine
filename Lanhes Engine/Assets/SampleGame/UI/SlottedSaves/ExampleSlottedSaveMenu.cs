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


    //TODO some sign that we are in save rather then load mode.
    public override void LoadMode() {
        isSaving = false;
    }

    public override void SaveMode() {
        isSaving = true;
    }

    internal override void SaveHeader(ref StreamWriter file) {
        //TODO shouldn't we be getting the data through the JSON document maybe, rather than direct data access?  
        int gold = PartyManager.GetParty().inventory.HowManyOfItem(DataManager.GetItemBySystemName("money"));
        file.WriteLine("Money: " + gold.ToString());
    }

    internal override void SkipHeader(ref StreamReader file) {
        file.ReadLine();//skip the gold number
    }

    public void SaveSelected(string path) {

        StartCoroutine(SaveSelectedBody(path));
        
    }

    public IEnumerator SaveSelectedBody(string path) {
        SelectionWindow confirmiationDialog = WindowManager.CreateConfirmDialog(null); //todo this should be this, not null
        while (!WindowManager.ContinuePlay()) {
            yield return null;
        }

        if (((SelectableBool)(confirmiationDialog.selected)).data) {
            if (isSaving) {
                //save over the selected slot
                SaveGame(path);
            } else {
                //load the selected slot
                LoadGame(path);
                CollapseMenu();
            }
        }

    }

}
