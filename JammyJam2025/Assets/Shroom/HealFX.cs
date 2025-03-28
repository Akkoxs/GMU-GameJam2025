using JetBrains.Annotations;
using UnityEngine;

public class HealFX : MonoBehaviour
{

    [SerializeField] public Animator anima; 
    [SerializeField] public SpriteRenderer spri;
    public Collider2D healingPad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        anima = GetComponent<Animator>();
        spri = GetComponent<SpriteRenderer>();

        spri.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            spri.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            spri.enabled = false;

        }
    }
}
