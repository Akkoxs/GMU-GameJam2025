using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup alphaGOUI; //GAME OVER UI

    public int gameOverTime = 5;   
    public GameObject gameoverUI; 
    private bool fadeDone = false; 
    Player player;

    public void TriggerGameOver(){
        player = GetComponent<Player>();
        player.enabled = false;
        player.animator.SetBool("isRevived", false); //dont live again
        player.animator.SetTrigger("isDead");
        if(!fadeDone){
            StartCoroutine(FadeUI(alphaGOUI, 0f, 1f, 1.0f));
        }
    }

    public IEnumerator FadeUI(CanvasGroup UI, float start, float end, float duration){ //deranged but it only works for canvasgroups nont canvas renderer 
        float elapsed = 0;
        yield return new WaitForSecondsRealtime(1.5f); //delay b/w death and UI fadeIn
        gameoverUI.SetActive(true);
        while(duration > elapsed){
            UI.alpha = (Mathf.Lerp(start, end, elapsed/duration)); 
            elapsed += Time.deltaTime;
        yield return null;
    UI.alpha = end;
    fadeDone = true;
        }

    }        

}
