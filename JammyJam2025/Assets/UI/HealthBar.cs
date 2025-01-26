using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider healthSlider;

    public void SetMaxHealth(int health){
        healthSlider.maxValue = health;
        healthSlider.value = health; //starting val is max 
    }
    public void SetHealth(int health){
        healthSlider.value = health;
    }

}
