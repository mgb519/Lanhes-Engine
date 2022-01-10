using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs an instance of the general case of battle, with a victory resulting in rewards and a loss resulting in game over. Should be overriden by games with more battle result states.
/// </summary>
public class GenericBattle : MapScript
{

    [Header("Don't use this! This is for testing purposes only. Make your own implmentation for your own game!")]

    [SerializeField]
    private IOpponentGroup enemies;
    public override IEnumerator Action() {
        yield return Body();
    }


    IEnumerator Body() {
        BattleManager.StartBattle(null);
        yield return new WaitUntil(() => !BattleManager.InBattle()); //wait for the battle to finish before continuing the script.
        BattleResult outcome = BattleManager.GetResultOfLastBattle();
        switch (outcome)
        {
            case BattleResult.Victory:
                //TODO give rewards
                break;
            case BattleResult.Loss:
                // game over screen
                GameSceneManager.GoToGameOver();
                break;
        }


    }


}
