using UnityEngine;

public class BaseMushroomTriggerScript : MonoBehaviour
{
    public Shroomaloom shroom;
    public GrowthSerum serum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shroom.serumDelivery && serum.pickedUpSerum)
        {
            shroom.serumDelivery = true;
            StartCoroutine(serum.Wait());
            this.gameObject.SetActive(false);
        }
    }
}
