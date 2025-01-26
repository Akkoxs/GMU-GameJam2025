using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Healing : LivingEntity
{
    [SerializeField] public HealthBar healthBar;
    [SerializeField] public int healthIncVal = 5;
    [SerializeField] public float healthIncTime = 0.3f;
    [SerializeField] public bool isHealing = false;

    private Coroutine healCoroutine;

    public void OnTriggerEnter2D(Collider2D collider){
        if((collider.gameObject.CompareTag("Player")) && (healCoroutine == null)){
            healCoroutine = StartCoroutine(Heal());
        }
    }

    public void OnTriggerExit2D(Collider2D collider){
        if((collider.gameObject.CompareTag("Player")) && (healCoroutine != null)){
            StopCoroutine(healCoroutine);
            healCoroutine = null; //reset
            isHealing = false;
        }
    }

    // heals the dude 
    public IEnumerator Heal(){
        while(healthBar.healthSlider.value < healthBar.healthSlider.maxValue){
            isHealing = true; //for the healthchanging animation 
            if((healthBar.healthSlider.value + healthIncVal) > 100){
                healthBar.healthSlider.value = healthBar.healthSlider.maxValue;
            }
            else{
                healthBar.healthSlider.value += healthIncVal;
            }
            yield return new WaitForSeconds(healthIncTime);
        }
        healCoroutine = null;
        isHealing = false; //reset
    }

}
