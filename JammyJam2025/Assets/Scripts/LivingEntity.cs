using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    public virtual void Start()
    {   
        health = maxHealth;
    }

    public virtual void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            // TODO Have some sort of dead texture/state instead?
            // Destroy(gameObject);
        }
    }
}
