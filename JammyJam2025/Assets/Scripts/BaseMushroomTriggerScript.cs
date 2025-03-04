using UnityEngine;

public class BaseMushroomTriggerScript : MonoBehaviour
{
    public Shroomaloom shroom;
    public GrowthSerum serum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shroom.serumDelivery && serum.pickedUpSerum)
        {
            //Debug.Log("here in base mushroom");
            shroom.serumDelivery = true;
            serum.droppedOffSerum = true;
            this.gameObject.SetActive(false);
        }
    }
}
