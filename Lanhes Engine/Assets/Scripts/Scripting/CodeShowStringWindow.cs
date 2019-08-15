using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeShowTextWindow : CodeBlock {

    public string text;
    public GameObject windowTemplate;
    public CodeBlock nextBlock;



    bool createdWindow = false;
    StringWindow window = null;

    //maybe have show string window use a variable?
    public override CodeBlock Execute(ScriptData locals, ScriptData globals) {
        if (createdWindow) {
            if (window != null) { return null; } //window has not been closed down, so we do not advance the script yet
            return nextBlock; //window was closed, so advance to the next code block
        } else {
            Debug.Log("Created String Window:" + text);
            window = GameObject.Instantiate(windowTemplate).GetComponent<StringWindow>();
            window.displayMe = text;
            return null;
        }

    }
}
