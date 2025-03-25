using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [SerializeField] public Shroomaloom shroom;
    [SerializeField] public int currPlatform = -1;

    [Header("Platform Settings")]
    public GameObject[] platforms;
    
    void Start()
    {
        foreach(GameObject platform in platforms){
            if (platform != null)
                platform.SetActive(true);
        }
    }

    public void Spawn(){
        if((currPlatform < platforms.Length)){
            platforms[currPlatform].SetActive(true);
            currPlatform++;
        }
        else if (currPlatform >= platforms.Length){
            Debug.Log("All platforms are already active.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (shroom.serumDelivery){
            Spawn();
        }
    }
}
