using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Unity.VisualScripting;
using System;


//Wrapper for a LocalizedString becuase for some fucking reason Visual Scripting/Bolt/Whatever the fuck this abomination is called refuses to recognise the idea of a LocalizedString that is a literal node rtaher than a graph variable.
[Serializable, Inspectable]
public class LocalizedStringNode
{
    //[Inspectable,SerializeField]
    public LocalizedString str;
}
