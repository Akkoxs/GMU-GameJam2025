using JetBrains.Annotations;
using UnityEngine;

public class HealFX : MonoBehaviour
{

    public bool introPauseAnim; //this sucks so bad but im lazy
    [SerializeField] public Animator anima; 
    [SerializeField] public SpriteRenderer spri;
    public Collider2D healingPad;

    void Start(){
        anima = GetComponent<Animator>();
        spri = GetComponent<SpriteRenderer>();
        spri.enabled = false;
    }

    public void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Player") && (!introPauseAnim)){
            spri.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player") && (!introPauseAnim)){
            spri.enabled = false;

        }
    }
}
