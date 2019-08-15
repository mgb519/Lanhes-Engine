using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CodeBlock : MonoBehaviour
{
    /// <summary>
    /// Get the node to call next.
    /// </summary>
    /// <returns>Next code block to execute, or null, if you must wait for something</returns>
    public abstract CodeBlock Execute(ScriptData locals,ScriptData globals);
}
