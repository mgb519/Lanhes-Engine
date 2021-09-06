using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//TODO there is TONNES that could be done here, like pages of slots. 
//TODO make this abstract?
public class SlottedSaveMenu : SaveMenu
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
        throw new System.NotImplementedException();
    }

    internal override void SkipHeader(ref StreamReader file) {
        throw new System.NotImplementedException();
    }
}
