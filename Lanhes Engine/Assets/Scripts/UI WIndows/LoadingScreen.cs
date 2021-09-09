using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadingScreen : MonoBehaviour
{


    /// <summary>
    /// When this is called, out progress towards finishing the load is denoted by progress
    /// </summary>
    /// <param name="progress">fraction of loading that is completed</param>
    public abstract void UpdateProgress(float progress);

}
