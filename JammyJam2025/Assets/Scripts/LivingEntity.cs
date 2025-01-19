using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    void Start()
    {   
        health = maxHealth;
    }

    virtual public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
