using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
    
    [SerializeField] private bool needSwitch = false;
    [SerializeField] private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        //WhichState
        if(Player.transform.position.y >= shroomMin){
            currentState = camStates.Shroom;
            targetCam = shroomCam;
        }
        else if((groundMax < Player.transform.position.y) && (Player.transform.position.y < shroomMin)){
            currentState = camStates.Platform;
            targetCam = platformCam;
        }
        else if(Player.transform.position.y <= groundMax){
            currentState = camStates.Ground; 
            targetCam = groundCam;
        }
        else{
            Debug.Log("Camera Selector does not know where to go.");
        }

        // //if the switch criteria coroutine is not running, start it, if it is already, stop it?
        // if (switchCoroutine == null){
        //     if (switchCoroutine != null){
        //         StopCoroutine(switchCoroutine);
        //     }
        //     switchCoroutine = StartCoroutine(NeedSwitch(targetCam));
        // }

        switchCoroutine = StartCoroutine(NeedSwitch(targetCam));

        //move camAnchor to the targetCam if needSwitch is true 
        if (needSwitch && !isMoving){
            needSwitch = false; //reset
            StartCoroutine(MoveCam(targetCam));
        }
    }

    public IEnumerator MoveCam(Transform camTarget){
        isMoving = true;
        float elapsedTime = 0f;
        while (!Mathf.Approximately(camAnchor.position.y, camTarget.position.y)){
            Vector2 startPos = camAnchor.position;
            Vector2 endPos = camTarget.position;
            camAnchor.position = Vector2.Lerp(startPos, endPos, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null; //wait until next frame 
        }
    isMoving = false;
    }

    private IEnumerator NeedSwitch(Transform camTarget){
        float timeNotMatching = 0f;
        while (true){
            if (!Mathf.Approximately(camAnchor.position.y, camTarget.position.y)){
                timeNotMatching += Time.deltaTime;
                if (timeNotMatching > timeUntilTransition){
                    needSwitch = true;
                    yield break; //break to else and reset 
                }
            }
            else {
                timeNotMatching = 0f;
            }
            yield return null;
        }
    }

}
