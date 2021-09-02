using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System;
using UnityEngine.UI;

//TODO this class is unfinshed
public class SlotlessSaveMenu : SaveMenu
{


    [SerializeField]
    private string saveDirectory;

    [SerializeField]
    private SaveInstanceButton saveSlotTemplate;

    [SerializeField]
    private Transform scrollviewContentBox;

    [SerializeField]
    private InputField saveNameField;

    [SerializeField]
    private Button saveButton;

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

            XmlDocument doc = new XmlDocument();
            XmlNode root = DataManager.instance.SaveToFile(doc);
            doc.AppendChild(root);
            doc.Save(path);
            //TODO add a header for saves to read rather than do full save prpcessing

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
}
