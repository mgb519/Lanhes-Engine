using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISelectable
{
    /// <summary>
    /// Creates a SelectionButton that represents the ISelectable. Must have a reference to the ISelectable.
    /// </summary>
    /// <returns></returns>
    SelectionButton Render();
}
