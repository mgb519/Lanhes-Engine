using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Runs an instance of the general case of battle, with a victory resulting in rewards and a loss resulting in game over. Should be overriden by games with more battle result states.
/// </summary>
public class GenericBattle : MonoBehaviour,MapScript
{
    //TODO this uses no MonoBehaviour methods, so why does it have the extra weight? A friend of mine had this exact problem, I'll ask him when I can get ahold of him...
    [SerializeField]
    private IOpponentGroup enemies;
    public void Action() {
        StartCoroutine(Body());
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
