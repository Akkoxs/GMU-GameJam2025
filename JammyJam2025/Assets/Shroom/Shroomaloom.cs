using System;
using System.Collections;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shroomaloom : LivingEntity
{
    public Transform middlePoint;
    private Rigidbody2D rb;
    private Transform trans;

    [SerializeField] public bool serumDelivery = false;
    [SerializeField] public bool platformSpawn = false;
    [SerializeField] float growSpeed = 10f;
    [SerializeField] float growDuration = 2;
    [SerializeField] public int serumCounter = 0;
    [SerializeField] public SpawnPlatform spawnplat;
    [SerializeField] public ShroomHealthBar healthBar;
    [SerializeField] public Animation anima;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start(){ //was override
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update(){
        healthBar.shroomhealthSlider.value = health;
        if(serumDelivery){
            platformSpawn = true;
            SerumDelivery();
        }    
    }

    public void SerumDelivery(){
        StartCoroutine(GrowMushroom());
        serumCounter++;
        spawnplat.currPlatform++;
        serumDelivery = false; //reset back 
        platformSpawn =  false; //reset back 
        }

    public IEnumerator GrowMushroom(){
        Vector2 startPos = trans.position; //gets start position every time method called 
        float targetHeight = startPos.y + growDuration*growSpeed;
        float timeElapsed = 0f;
        while (timeElapsed < growDuration){
            trans.position = new Vector2(startPos.x, Mathf.Lerp(startPos.y, targetHeight, timeElapsed/growDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait for next frame 
        }
        
        trans.position = new Vector2(startPos.x, targetHeight);


    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

}
