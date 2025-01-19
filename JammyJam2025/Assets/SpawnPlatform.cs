using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [SerializeField] public Shroomaloom shroom;
    [SerializeField] public int currPlatform = 0;

    [Header("Platform Settings")]
    public GameObject[] platforms;
    
    void Start()
    {
        foreach(GameObject platform in platforms){
            if (platform != null)
                platform.SetActive(false);
        }
    }

    public void Spawn(){
        if((currPlatform < platforms.Length)){
            Debug.Log("POOPS");
            platforms[currPlatform].SetActive(true);
            currPlatform++;
        }
        else if (currPlatform >= platforms.Length){
            Debug.Log("All platforms are already active.");
        }
    }

// reference game object for transform position of island 
// serialize all in order
// store in array 
// reference SerumDeliveries count from grow shroom script
// every time serum goes up, unhide a platform 


    // Update is called once per frame
    void Update()
    {
        if (shroom.serumDelivery){
            Spawn();
        }
    }
}
