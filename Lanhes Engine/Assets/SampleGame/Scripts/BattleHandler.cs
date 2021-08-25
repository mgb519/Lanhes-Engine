using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    //placeholder class that manages combat
    //In the sample game, combat is decied by the player, but in a real game, this class would likely orchestrate combat i.e turn scheduling

    public void Win()
    {
        Debug.Log("Ending battle with victory");
        BattleManager.EndBattle(BattleResult.Victory);
    }

    public void Lose()
    {
        Debug.Log("Ending battle with loss");
        BattleManager.EndBattle(BattleResult.Loss);
    }
}
