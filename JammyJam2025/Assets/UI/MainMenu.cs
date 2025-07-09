using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private RawImage BlackScreen;
    [SerializeField] private Transform PlayerSprite;
    [SerializeField] private float centerX;
    [SerializeField] private float centerY;

    private Color BlFade;

    private float blackScreenTime = 1.3f;
    private float fadeDuration = 1.5f;


    public void Start(){
        Vector3 centerPos = new Vector3(centerX, centerY, 0f);
        BlFade = BlackScreen.color;
        BlFade.a = 1f;
        BlackScreen.color = BlFade;
        StartCoroutine(FadeScreen(BlFade, 1f, 0f, fadeDuration, blackScreenTime));
    }

    public void Update(){
        //Fallsway
    }

    private IEnumerator FallSway(){
        yield return new WaitForSecondsRealtime(blackScreenTime);
        //while we are not at target height, fall

        //while true, sway
            //maybe have a lerp movment to a new Y
            //new Y is calculated after place is reached as a RAND between 2 limits
        yield return null; 
    }

    public IEnumerator FadeScreen(Color UI, float start, float end, float duration, float delay){ //copy and pasted code because im stupid
        yield return new WaitForSecondsRealtime(delay);
        float elapsed = 0;
        while(duration > elapsed){
            UI.a = (Mathf.Lerp(start, end, elapsed / duration));
            BlackScreen.color = UI;
            elapsed += Time.deltaTime;
        yield return null;
        }
    UI.a = end;
    BlackScreen.color = UI;
    BlackScreen.gameObject.SetActive(false);
    }

    public void PlayGame(){
        //BlFade = BlackScreen.color;
        StartCoroutine(FadeScreen(BlFade, 0f, 1f, fadeDuration, 0f));
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame(){
        Application.Quit();
    }

}
