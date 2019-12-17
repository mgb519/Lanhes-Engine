using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDialogue : MonoBehaviour {
    GameObject player;

    public List<ItemCost> dummyShopBuyData;
    public List<ItemCost> dummyShopSellData;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == player) {
            //TODO: have script triggers and scripts be seperate
            StartCoroutine(StringTestBody());
        }
    }


    IEnumerator StringTestBody() {

        WindowManager.CreateStringWindow("Hey there!");
        yield return new WaitUntil(() => Time.timeScale != 0);

        Debug.Log("returned from first dialogue");

        WindowManager.CreateStringWindow("I'm going to make you choose between Foo and Bar!");
        yield return new WaitUntil(() => Time.timeScale != 0);
        

        string selection = null;
        while (selection != "Foo" || selection != "Bar") {
            SelectionWindow s = WindowManager.CreateStringSelection(new List<string> { "Foo", "Bar", "Baz" });

            yield return new WaitUntil(() => Time.timeScale != 0);
            selection = ((SelectableString)(s.selected)).data;

            if (selection == "Foo" || selection == "Bar") {
                WindowManager.CreateStringWindow("You chose " + selection + ".");
                yield return new WaitUntil(() => Time.timeScale != 0);
                break;
            } else {
                WindowManager.CreateStringWindow("Baz? You chose Baz? That's not fair!");
                yield return new WaitUntil(() => Time.timeScale != 0);
                WindowManager.CreateStringWindow("Choose something else!");
                yield return new WaitUntil(() => Time.timeScale != 0);
            }
        }
        WindowManager.CreateStringWindow("Let's shop!");
        yield return new WaitUntil(() => Time.timeScale != 0);

        WindowManager.CreateShopWindow(dummyShopBuyData, dummyShopSellData, player.GetComponent<PlayerController>().inventory);
        yield return new WaitUntil(() => Time.timeScale != 0);

        yield break;
    }


}


