using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTerminateScript : CodeBlock {
    public override CodeBlock Execute(ScriptData locals, ScriptData globals) {
        Debug.LogWarning("NextNode() called from Script Terminator; this should not occur");
        return null;
    }
}
