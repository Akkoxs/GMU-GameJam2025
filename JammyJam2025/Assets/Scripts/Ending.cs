using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Ending : MonoBehaviour
{
    [TextArea] public List<string> diaText;

    [SerializeField] private float diaSpeed = 0.01f;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Fading")]
    [SerializeField] private RawImage BLKSCRN; //needed to use UI elem. to cover everything
    private Color BlackFade; //is a colour, colours have alpha property while RawImages dont 

    void Start() {
        BLKSCRN.gameObject.SetActive(true);
        BlackFade = BLKSCRN.color;
        BlackFade.a = 1f; //ensure starts at opaque
        BLKSCRN.color = BlackFade;
        StartCoroutine(FadeScreen(BlackFade, 1f, 0f, 2.5f, 0.00125f));
    }
    
    public void ActivateSunSpeech() {
        StartCoroutine(SunSpeech());
    }

    private IEnumerator SunSpeech(){
        int randomDia = Random.Range(0, diaText.Count);

        for (int i = 0; i < diaText[randomDia].Length + 1; i++){
            text.text = diaText[randomDia].Substring(0, i);
            yield return new WaitForSecondsRealtime(diaSpeed);
            
        }
    }

    //next time make a static non-monobehaviour class with static methods as a UI helper not this bullshit 
    public IEnumerator FadeScreen(Color UI, float start, float end, float duration, float fadespeed) {
        float elapsed = 0;
        yield return new WaitForSecondsRealtime(2.25f);
        while (duration > elapsed) {
            UI.a = Mathf.Lerp(start, end, elapsed / duration);
            BLKSCRN.color = UI;
            elapsed += Time.deltaTime;
            yield return new WaitForSecondsRealtime(fadespeed);
        }
        UI.a = end;
        BLKSCRN.color = UI;
        BLKSCRN.gameObject.SetActive(false);
    }

    public void HOME() {
    SceneManager.LoadSceneAsync(0);
    }
}
