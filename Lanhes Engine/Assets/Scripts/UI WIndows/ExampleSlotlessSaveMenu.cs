using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System;
using UnityEngine.UI;
using TMPro;
//TODO The user has to override this class
public class ExampleSlotlessSaveMenu : SaveMenu
{


    [SerializeField]
    private string saveDirectory;

    [SerializeField]
    private SaveInstanceButton saveSlotTemplate;

    [SerializeField]
    private Transform scrollviewContentBox;

    [SerializeField]
    private TMP_InputField saveNameField; 

    [SerializeField]
    private Button saveButton;


    //TODO this should not exist, get this statically
    public InventoryItem goldItem;

    private bool saving = true;

    void Awake() {
        //Time.timeScale = 0f;

        //populate the list
        RefreshList();
    }


    public void SaveSelected(string path) {
        if (saving) {
            //save over the selected slot

            //TODO confirmation dialog
            SaveGame(path);
            RefreshList();
        } else {
            //load the selected slot

            //TODO confirmation dialogue
            LoadGame(path);
            CollapseMenu();
        }
    }


    public void SaveNew() {
        DateTimeOffset dto = new DateTimeOffset(DateTime.Now);
        string path = saveDirectory + "/" + dto.ToUnixTimeSeconds().ToString() + ".lhsav";
        SaveSelected(path);
    }



    private void RefreshList() {
        //TODO: object pooling would be a nice idea, i.e if we already have some eixtsing slots on refresh, just call SetPath on them rather than Destroy and Create


        //refresh the list of saves
        for (int i = scrollviewContentBox.childCount - 1; i >= 0; --i) {
            GameObject.Destroy(scrollviewContentBox.GetChild(i).gameObject);
        }
        scrollviewContentBox.DetachChildren();



        IEnumerable<string> filepaths = Directory.EnumerateFiles(saveDirectory, "*.lhsav");
        List<FileInfo> fileInfos = new List<FileInfo>();
        foreach (string p in filepaths) {
            fileInfos.Add(new FileInfo(p));
        }

        fileInfos.Sort((x, y) => y.LastWriteTime.CompareTo(x.LastWriteTime));

        foreach (FileInfo f in fileInfos) {
            SaveInstanceButton fileSlot = Instantiate(saveSlotTemplate);
            fileSlot.transform.SetParent(scrollviewContentBox);
            fileSlot.SetPath(f.FullName);
        }


    }

    public override void LoadMode() {
        saving = false;
        //grey out the new save button
        saveButton.interactable = false;

    }

    public override void SaveMode() {
        saving = true;
        //enable the new save button
        saveButton.interactable = true;
    }

    public override void DeleteSave(string path) {
        //TODO throw a confirmation dialogue
        File.Delete(path);
        RefreshList();
    }

    internal override void SaveHeader(ref StreamWriter file) {
        //TODO shouldn't we be getting the data through the XML document maybe, rather than direct data access?
        string saveName = saveNameField.text==null?"no notes":saveNameField.text;
        file.WriteLine(saveName);
        int gold = PartyManager.GetParty().inventory.HowManyOfItem(goldItem);//TODO get Gold statically
        file.WriteLine("Money: "+gold.ToString());

    }

    internal override void SkipHeader(ref StreamReader file) {
        file.ReadLine();//skip the save name
        file.ReadLine();//skip the gold number
    }
}
