using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public HealthBar player_healthBar; 
    public float hp;

    void Update(){
        PlayerHP();
    }
    public void PlayerHP(){
        hp = player_healthBar.healthSlider.value;
    }
}
