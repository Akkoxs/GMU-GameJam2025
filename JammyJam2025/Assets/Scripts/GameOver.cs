using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public float gameOverTime = 2f;   


    public void TriggerGameOver(){
        StartCoroutine(DeathWaitTime());
        SceneManager.LoadSceneAsync("GameOver");
    } 

    public IEnumerator DeathWaitTime(){
        yield return new WaitForSecondsRealtime(gameOverTime);
    }


}
