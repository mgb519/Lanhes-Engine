using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Trigger : MonoBehaviour
{
    [SerializeField,SerializeReference]
    internal MapScript scriptToCall;
}
