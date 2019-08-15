using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//calls a script starting from a codeBlock. Execute() function of this is called to start a script.
public class ScriptExecutor : MonoBehaviour {




    public CodeBlock startCommand;

    private ScriptData localVariables = new ScriptData();

    private CodeBlock currentCommand;

    public void Execute() {
        StartCoroutine(ExecuteScript());
    }

    private IEnumerator ExecuteScript() {
        currentCommand = startCommand;
        while (!(currentCommand is CodeTerminateScript)) {
            CodeBlock nextCommand = currentCommand.Execute(localVariables, SceneManager.globalVariables);
            if (nextCommand != null) {
                currentCommand = nextCommand;
            } else {
                yield return null;
            }
        }
    }
}
