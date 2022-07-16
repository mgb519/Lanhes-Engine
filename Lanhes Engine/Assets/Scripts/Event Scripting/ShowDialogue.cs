using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Localization;
using UnityEngine;

public class ShowDialogue : WaitUnit
{

    [DoNotSerialize] // No need to serialize ports
    public ValueInput text; //The string we input

    

    //TODO portraits,speaker names and the like

    protected override void Definition() //The method to set what our node will be doing.
    {
        base.Definition();
        text = ValueInput<LocalizedString>(nameof(text));
        Requirement(text, enter);
    }

    protected override IEnumerator Await(Flow flow) {
        
        WindowManager.CreateStringWindow(flow.GetValue<LocalizedString>(text), null);
        yield return new WaitUntil(() => WindowManager.ContinuePlay());

        yield return exit;

    }
}