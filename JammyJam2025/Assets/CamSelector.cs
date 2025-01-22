using Unity.Cinemachine;
using UnityEngine;

public class CamSelector : MonoBehaviour // pans the camera to focus during gameplay
{

    public CinemachineCamera cam;
    public Transform trans;

    //game objects for cam anchors 
    [Header("Camera Anchors")]
    [SerializeField] public Transform shroomCam;
    [SerializeField] public Transform platformCam;
    [SerializeField] public Transform groundCam;


    //min max y-dim trigger points for camera pan 
    [Header("Min Max y-Dimension Triggers")]
    [SerializeField] public float shroomMin;
    [SerializeField] public float platformMax;
    [SerializeField] public float platformMin;
    [SerializeField] public float groundMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<CinemachineCamera>(); //get component even tho there is no cam component attached to this script ? can we serialize/attach?
        trans = GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void WhichCam(trans.position){
        // which cam to use ? relative to location 
    //}



}
