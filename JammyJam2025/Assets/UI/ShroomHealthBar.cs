using UnityEngine;
using UnityEngine.UI;

public class ShroomHealthBar : MonoBehaviour
{
    [SerializeField] public Slider shroomhealthSlider;

    public void SetMaxHealth(int health){
        shroomhealthSlider.maxValue = health;
        shroomhealthSlider.value = health; //starting val is max 
    }
    public void SetHealth(int health){
        shroomhealthSlider.value = health;
    }
}
