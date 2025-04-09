using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool limboTime = false;
    public int gameOverTime = 5;   
    Player player;

    public void TriggerGameOver(){
        limboTime = true;
        player = GetComponent<Player>();
        player.velocity = new Vector3 (0,0,0);
        player.animator.SetTrigger("isDead");
        StartCoroutine(LimboTime());

        //inshallah we shall add audio here 
    } 

    private IEnumerator LimboTime(){
        if(limboTime){
            yield return new WaitForSecondsRealtime(gameOverTime);
            limboTime = false;
            SceneManager.LoadSceneAsync("GameOver");
        }
    }


}
