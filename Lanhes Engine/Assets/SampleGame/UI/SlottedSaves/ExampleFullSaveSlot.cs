using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ExampleFullSaveSlot : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    private string path;
    public void SetPath(string newPath) {
        path = newPath;
        //set up slot visuals
        timeText.text = new FileInfo(path).LastWriteTime.ToString(); 
        StreamReader reader = new StreamReader(path);
        goldText.text = reader.ReadLine();
        reader.Close();
    }

}
