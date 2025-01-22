using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthSerum : MonoBehaviour
{
    public bool pickedUpSerum;
    public bool droppedOffSerum;
    public GameObject baseMushroomTrigger;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        droppedOffSerum = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (droppedOffSerum)
        {
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUpSerum)
        {
            pickedUpSerum = true;
            baseMushroomTrigger.SetActive(true);
            spriteRenderer.enabled = false;
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(5f);
        pickedUpSerum = false;
        droppedOffSerum = false;
        baseMushroomTrigger.SetActive(false);
        spriteRenderer.enabled = true;
    }
}
