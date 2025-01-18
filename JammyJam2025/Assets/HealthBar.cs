using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    public void SetMaxHealth(int health){
        healthSlider.maxValue = health;
        healthSlider.value = health; //starting val is max 
    }
    public void SetHealth(int health){
        healthSlider.value = health;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
