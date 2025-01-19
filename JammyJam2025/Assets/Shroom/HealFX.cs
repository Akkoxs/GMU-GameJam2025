using JetBrains.Annotations;
using UnityEngine;

public class HealFX : MonoBehaviour
{

    public Animator anima; 
    public SpriteRenderer spri;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        anima = GetComponent<Animator>();
        spri = GetComponent<SpriteRenderer>();

        spri.enabled = false;
    }

    // Update is called once per frame
    public void TriggerHealFX(){
        anima.SetTrigger("isHealing");

    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            TriggerHealFX();
            spri.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            TriggerHealFX();
            spri.enabled = false;

        }
    }
}
