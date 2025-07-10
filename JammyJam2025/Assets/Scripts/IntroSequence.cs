using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] public bool enableIntro;
    [SerializeField] private float limboTime = 0.33f; //the time between the player input & allowing you to start 
    [SerializeField] private Healing healing;
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private GameObject ShroomHealthBar;
    [SerializeField] private GameObject titleUI;
    [SerializeField] private CanvasRenderer titleCanvas;
    [SerializeField] private HealFX healfx;
    [SerializeField] private RawImage BLKSCRN;

    private Player player;
    public MainMenu mainMenu;
    private Coroutine corot = null;
    private Color BlackFade;

    void Start()
    {
        BlackFade = BLKSCRN.color;
        BlackFade.a = 1f;
        BLKSCRN.color = BlackFade;
        player = GetComponent<Player>();
        enableIntro = true;
        healfx.introPauseAnim = true;
        HideHealthBars(true);
        titleUI.SetActive(false);
        StartCoroutine(FadeScreen(BlackFade, 1f, 0f, 2.5f));
    }

    void Update()
    {
        if (!enableIntro) return;
        {
            player.velocity.y += player.gravity * Time.deltaTime;
            player.controller.Move(player.velocity * Time.deltaTime);
            if ((player.controller.collisionInfo.below) && (corot == null) && (enableIntro))
            {
                healing.isHealing = false;
                healing.introPause = true;
                corot = StartCoroutine(PlayerTimeout());
            }
        }
    }

    public IEnumerator PlayerTimeout()
    {
        player.player_healthBar.healthSlider.value = 1;
        player.animator.SetBool("isDead", true);
        titleUI.SetActive(true);
        yield return StartCoroutine(FadeInTitle(0f, 1f, 1.0f)); //yield return statements wait for a return of the corout before proceeding 
        yield return new WaitUntil(() => Input.anyKey);
        player.animator.SetBool("isDead", false);
        player.animator.SetBool("isRevived", true);
        yield return new WaitForSecondsRealtime(limboTime);
        healing.isHealing = true;
        HideHealthBars(false);
        healing.introPause = false;
        healfx.introPauseAnim = false;
        yield return StartCoroutine(FadeInTitle(1f, 0f, 1.5f));
        enableIntro = false;
        titleUI.SetActive(false);
    }

    public void HideHealthBars(bool command)
    {
        if (command == true)
        { //hide health bars
            HealthBar.SetActive(false);
            ShroomHealthBar.SetActive(false);
        }

        else if (command == false)
        { //show health bars
            HealthBar.SetActive(true);
            ShroomHealthBar.SetActive(true);
        }

        else
        {
            Debug.Log("HideHealthBars() is not receiving true/false arg");
        }
    }

    public IEnumerator FadeInTitle(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            titleCanvas.SetAlpha(Mathf.Lerp(start, end, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        titleCanvas.SetAlpha(end); //when finished fading, set it and keep it 
    }

    //messsed it up and didnt make it static so now I gotta copy this again here:
    public IEnumerator FadeScreen(Color UI, float start, float end, float duration){
        float elapsed = 0;
        yield return new WaitForSecondsRealtime(0.25f);
        while (duration > elapsed)
        {
            UI.a = Mathf.Lerp(start, end, elapsed / duration);
            BLKSCRN.color = UI;
            elapsed += Time.deltaTime;
            yield return null;
        }
        UI.a = end;
        BLKSCRN.color = UI;
        BLKSCRN.gameObject.SetActive(false);
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
