using System.Collections;
using UnityEngine;

public class CamSelector : MonoBehaviour // pans the camera to focus during gameplay
{

    [SerializeField] public GameObject Player;
    [SerializeField] public float transitionTime;
    [SerializeField] public float timeUntilTransition;

    //game objects for cam anchors 
    [Header("Camera Anchors")]
    [SerializeField] public Transform shroomCam;
    [SerializeField] public Transform platformCam;
    [SerializeField] public Transform groundCam;
    [SerializeField] public Transform camAnchor; 

    //min max y-dim trigger points for camera pan 
    [Header("Min Max y-Dimension Triggers")]
    [SerializeField] public float shroomMin;
    [SerializeField] public float groundMax;

    public enum camStates{Shroom, Platform, Ground}
    public camStates currentState;
    private Coroutine switchCoroutine;

    Transform targetCam;
    
    [Header("Switching Params")]
    [SerializeField] private bool needSwitch = false;
    [SerializeField] private bool isMoving = false;
    float timeNotMatching = 0f;

    // Update is called once per frame
    void Update(){
        //WhichState
        if (Player.transform.position.y >= shroomMin) {
            currentState = camStates.Shroom;
            targetCam = shroomCam;
        }
        else if ((groundMax < Player.transform.position.y) && (Player.transform.position.y < shroomMin)) {
            currentState = camStates.Platform;
            targetCam = platformCam;
        }
        else if (Player.transform.position.y <= groundMax) {
            currentState = camStates.Ground;
            targetCam = groundCam;
        }
        else {
            Debug.Log("Camera Selector does not know where to go.");
        }

        if (!Mathf.Approximately(camAnchor.position.y, targetCam.position.y)) {
            timeNotMatching += Time.deltaTime;
            if (timeNotMatching >= timeUntilTransition && !isMoving) {
                StartCoroutine(MoveCam(targetCam));
                timeNotMatching = 0f;
            }
            
        }
    }

    public IEnumerator MoveCam(Transform camTarget) {
        float elapsedTime = 0f;
        isMoving = true;
        while (!Mathf.Approximately(camAnchor.position.y, targetCam.position.y)) {
            Vector2 startPos = new Vector2(Player.transform.position.x, camAnchor.position.y);
            Vector2 endPos = new Vector2(Player.transform.position.x, camTarget.position.y);
            camAnchor.position = Vector2.Lerp(startPos, endPos, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null; //wait until next frame 
        }
        isMoving = false;
        camAnchor.position = new Vector2(Player.transform.position.x, camTarget.position.y);
    }

}
