using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Ending : MonoBehaviour
{
    [TextArea] public List<string> diaText;

    [SerializeField] private float diaSpeed = 0.01f;
    [SerializeField] private TextMeshProUGUI text;


    public void ActivateSunSpeech(){
        StartCoroutine(SunSpeech());
    }

    private IEnumerator SunSpeech()
    {
        int randomDia = Random.Range(0, diaText.Count);

        for (int i = 0; i < diaText[randomDia].Length + 1; i++)
        {
            text.text = diaText[randomDia].Substring(0, i);
            yield return new WaitForSecondsRealtime(diaSpeed);
        }
    }

    // Sunspore says: Don't make others suffer for your personal hatred, stuff like that LOL. Evangelion Font.
    // Did you dream?
    // Remember your promise.
    // The wind will come again.
    // Afterall, why not me?
    // We knew eachother, long ago, but you have forgotten 
    // Do you remember, my love?
    // Rage. Rage against the dying of the light.
    // Did you exchange, a walk-on part in the war, for a lead role in a cage?
    // I wish you were here.
    // I am so glad you are here.
    // 

}
