using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    public LivingEntity player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
