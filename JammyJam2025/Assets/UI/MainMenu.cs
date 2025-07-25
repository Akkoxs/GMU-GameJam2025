using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private RawImage BlackScreen;
    [SerializeField] private Transform pS; //playerSPrite
    [SerializeField] private Transform cp; //centerpoint
    [SerializeField] private Transform LHS_Lim;
    [SerializeField] private Transform RHS_Lim;

    static Color BlFade;

    private float tgt; //target
    private float blackScreenTime = 1.3f;
    private bool isFalling = false; 

    public void Start()
    {            
        BlFade = BlackScreen.color;
        BlFade.a = 1f;
        BlackScreen.color = BlFade;
        StartCoroutine(FadeScreen(BlFade, 1f, 0f, 1f, blackScreenTime, 0f, false, false));
    }

    public void Update(){
        if (!isFalling){
            StartCoroutine(Fall(pS.position.y, cp.position.y, 3f));
            StartCoroutine(FallSway());
            isFalling = true;
        }
    }

    private IEnumerator Fall(float start, float end, float duration){
        Vector3 pos = pS.position;
        float elapsed = 0;

        while(elapsed < duration){
            pos.y = Mathf.Lerp(start, end, elapsed/duration);
            pS.position = new Vector3(pS.position.x, pos.y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        pos.y = end;
        pS.position = new Vector3(pS.position.x, pos.y, 0f);
        }
    

    private IEnumerator FallSway(){
        float transition; 
        Vector3 pos = pS.position;
        yield return new WaitForSecondsRealtime(2.5f);
        while(true){
            transition = Random.Range(1f, 4f);
            tgt = Random.Range(LHS_Lim.position.x, RHS_Lim.position.x);
            float swayTimer = 0;
            float sp = pS.position.x; //startpoint

            while(swayTimer < transition){
                pos.x = Mathf.Lerp(sp, tgt, swayTimer/transition);
                pS.position = new Vector3(pos.x, pS.position.y, 0f);
                swayTimer += Time.deltaTime;
                yield return null;
            }
            pos.x = tgt;
            pS.position = new Vector3(pos.x, pS.position.y, 0f);
        }
    }



public IEnumerator FadeScreen(Color UI, float start, float end, float duration, float delay, float endDelay, bool isActive, bool endScene)
{ //copy and pasted code because im stupid
    yield return new WaitForSecondsRealtime(delay);
    float elapsed = 0;
    while (duration > elapsed){
        UI.a = Mathf.Lerp(start, end, elapsed / duration);
        BlackScreen.color = UI;
        elapsed += Time.deltaTime;
        yield return null;
    }
    UI.a = end;
    BlackScreen.color = UI;
    BlackScreen.gameObject.SetActive(isActive);
    yield return new WaitForSecondsRealtime(endDelay);
        if (endScene){
            SceneManager.LoadSceneAsync(1); //this is horrible but idc anymore 
            yield break;
        }
    }

    public void PlayGame(){
        BlackScreen.gameObject.SetActive(true);
        BlFade = BlackScreen.color;
        BlFade.a = 0f;
        BlackScreen.color = BlFade;
        StartCoroutine(FadeScreen(BlFade, 0f, 1f, 1.5f, 0f, blackScreenTime, true, true));
    }

    public void QuitGame(){
        Application.Quit();
    }

}
