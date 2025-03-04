using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;


public class HealthChangeAnim : MonoBehaviour
{
    [SerializeField] public float healthChangeAnimationTime = 0.1f;
    [SerializeField] public HealthBar healthBar; 
    [SerializeField] public Healing healingScript;
    [SerializeField] public Player player;
    [SerializeField] public bool isChanging;

    [SerializeField] public Image healthFill; 

    public Color ogColor;
    private Coroutine healthChangeCorout;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isChanging = false;
        ogColor = healthFill.color; //store the og color
    }

    // Update is called once per frame
    void Update()
    {
        if((healingScript.isHealing || player.isDying) && (healthChangeCorout == null)){
            isChanging = true;
            healthChangeCorout = StartCoroutine(HealthChangeCorout());
        }
        else if((!healingScript.isHealing && !player.isDying) && (healthChangeCorout != null)){ //if its runing, stop the coroutine when youre off healing pad 
            StopCoroutine(healthChangeCorout);
            healthFill.color = ogColor;
            healthChangeCorout = null;
            isChanging = false;
        }

    }

    public IEnumerator HealthChangeCorout(){
        while(isChanging){
            healthFill.color = Color.white; 
            yield return new WaitForSeconds(healthChangeAnimationTime); 
            healthFill.color = ogColor;
            yield return new WaitForSeconds(healthChangeAnimationTime); 

        }
    }


}


