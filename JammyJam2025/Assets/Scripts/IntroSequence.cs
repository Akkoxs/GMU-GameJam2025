using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] public bool enableIntro;
    [SerializeField] private float revivalTime = 1.5f;
    [SerializeField] private Healing healing;
    [SerializeField] private GameObject HealthBar; 
    [SerializeField] private GameObject ShroomHealthBar; 
    [SerializeField] private GameObject titleUI;
    [SerializeField] private CanvasRenderer titleCanvas;


    private Player player;
    private Coroutine corot = null; 

    void Start(){
        player = GetComponent<Player>();
        enableIntro = true;
        HideHealthBars(true);
        titleUI.SetActive(false);
    }

    void Update(){
        if (!enableIntro) return; {
            player.velocity.y += player.gravity * Time.deltaTime;
            player.controller.Move(player.velocity * Time.deltaTime);
            if((player.controller.collisionInfo.below) && (corot == null) && (enableIntro)){
                healing.isHealing = false; 
                healing.introPause = true;
                corot = StartCoroutine(PlayerTimeout());
            }
        }
    }

    public IEnumerator PlayerTimeout(){
        player.player_healthBar.healthSlider.value = 1;
        player.animator.SetBool("isDead", true);
        titleUI.SetActive(true);
        yield return StartCoroutine(FadeInTitle(0f, 1f, 1.0f)); //yield return statements wait for a return of the corout before proceeding 
        yield return new WaitForSecondsRealtime(revivalTime);
        healing.introPause = false;
        healing.isHealing = true;
        player.animator.SetBool("isDead", false);
        player.animator.SetBool("isRevived", true);
        HideHealthBars(false);
        //Title card UI flag 
        enableIntro = false;
        yield return StartCoroutine(FadeInTitle(1f, 0f, 1.5f));
        titleUI.SetActive(false);
    }

    public void HideHealthBars(bool command){
        if (command == true){ //hide health bars
            HealthBar.SetActive(false);
            ShroomHealthBar.SetActive(false);
        }

        else if (command == false){ //show health bars
            HealthBar.SetActive(true);
            ShroomHealthBar.SetActive(true);  
        }

        else{
            Debug.Log("HideHealthBars() is not receiving true/false arg");
        }
    }

    public IEnumerator FadeInTitle(float start, float end, float duration){
        float elapsed = 0f;
        while (elapsed < duration){
            titleCanvas.SetAlpha(Mathf.Lerp(start, end, elapsed/duration));
            elapsed += Time.deltaTime;
        yield return null;
        }
        titleCanvas.SetAlpha(end); //when finished fading, set it and keep it 
    }

    // start off falling 
    // make sure player cannot move 
    // hits ground, starts sequence.
    // hp = like 1 
    // death anim.
    // pause
    // revival anim. + healing anim. 
    // title card.
    // fuck yeah you can play now 
    // disable update script.
}
