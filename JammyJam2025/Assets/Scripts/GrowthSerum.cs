using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GrowthSerum : MonoBehaviour
{
    public bool pickedUpSerum;
    public bool droppedOffSerum;
    public int sidePicker;
    public float LHSnewPos;
    public float RHSnewPos;
    public GameObject baseMushroomTrigger;

    private Transform trans;
    private SpriteRenderer spriteRenderer;

    [SerializeField] public Image inventorySerum; 

    [SerializeField] public float LHS_maxSpawn;
    [SerializeField] public float LHS_minSpawn;
    [SerializeField] public float RHS_minSpawn;
    [SerializeField] public float RHS_maxSpawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        droppedOffSerum = false;
        inventorySerum.enabled = false; 
        spriteRenderer = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
    }

    public void Update()
    {
        if (droppedOffSerum)
        {
            inventorySerum.enabled = false;
            sidePicker = Random.Range(1,3);
            LHSnewPos = Random.Range(LHS_minSpawn, LHS_maxSpawn);
            RHSnewPos = Random.Range(RHS_minSpawn, RHS_maxSpawn);
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUpSerum)
        {
            pickedUpSerum = true;
            inventorySerum.enabled = true;
            baseMushroomTrigger.SetActive(true);
            spriteRenderer.enabled = false;
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(5f);
        RandomSpawner();
        pickedUpSerum = false;
        droppedOffSerum = false;
        baseMushroomTrigger.SetActive(false);
        spriteRenderer.enabled = true;
    }

    public void RandomSpawner(){
        if (sidePicker == 1){ //LHS
            transform.position = new Vector2(LHSnewPos, trans.position.y);
        }

        else if (sidePicker == 2){ //RHS
            transform.position = new Vector2(RHSnewPos, trans.position.y);
        }
    }

}
