using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDialogue : MonoBehaviour {
    GameObject player;

    public List<ItemCost> dummyShopData;

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

        SceneManager.CreateStringWindow("Hey there!");
        yield return new WaitUntil(() => Time.timeScale != 0);

        Debug.Log("returned from first dialogue");

        SceneManager.CreateStringWindow("I'm going to make you choose between Foo and Bar!");
        yield return new WaitUntil(() => Time.timeScale != 0);


        string selection = null;
        while (selection != "Foo" || selection != "Bar") {
            StringSelectWindow s = SceneManager.CreateStringSelectWindow(new List<string> { "Foo", "Bar", "Baz" });

            yield return new WaitUntil(() => Time.timeScale != 0);
            selection = s.selected;

            if (selection == "Foo" || selection == "Bar") {
                SceneManager.CreateStringWindow("You chose " + selection + ".");
                yield return new WaitUntil(() => Time.timeScale != 0);
                break;
            } else {
                SceneManager.CreateStringWindow("Baz? You chose Baz? That's not fair!");
                yield return new WaitUntil(() => Time.timeScale != 0);
                SceneManager.CreateStringWindow("Choose something else!");
                yield return new WaitUntil(() => Time.timeScale != 0);
            }
        }
        SceneManager.CreateStringWindow("Let's shop!");
        yield return new WaitUntil(() => Time.timeScale != 0);

        SceneManager.CreateShopWindow(dummyShopData, player.GetComponent<PlayerController>().inventory);
        yield return new WaitUntil(() => Time.timeScale != 0);

        yield break;
    }


}


