using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Oculus.Interaction;
using OVR;
using UnityEditor;
using UnityEngine;
 

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;
    [SerializeField] public GameObject right_controller_prefab;
    [SerializeField] public GameObject left_controller_prefab;
    [SerializeField] public GameObject rocket;
    [SerializeField] public AudioSource wind;

    [SerializeField] public GameObject parachute;
   
    
    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;
    private float NextFire = 0;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        parachute.SetActive(false);
        wind.volume = 1.0f;
    }

    void Update()
    {
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController); 
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);
        
        if (rb.velocity.magnitude >= 1 && !wind.isPlaying)
        {
            
            wind.Play();
        }
        else if (wind.isPlaying && rb.velocity.magnitude <= 1)
        {
         
            wind.Stop();
            
            
        }
        else
        {
            
        }
        
        if (hmd.transform.position.y <= left_controller_prefab.transform.position.y 
            && hmd.transform.position.y <= right_controller_prefab.transform.position.y 
            && rb.velocity.y < 0.01)
        {
            parachute.SetActive(true);
            rb.drag = 1.5f;
        }
        else
        {
            parachute.SetActive(false);
            rb.drag = 0;
        }
        
        
        if (rightTriggerValue >= 0.95f && (Time.time > NextFire))
        {
            NextFire = Time.time + .5f;
            LaunchRocket(right_controller_prefab);
        }
        
        if (leftTriggerValue >= 0.95f && (Time.time > NextFire))
        {
            NextFire = Time.time + .5f;
            LaunchRocket(left_controller_prefab);
        }
        
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                this.transform.position = parkourCounter.currentRespawnPos;
            }
        }
    }

    private void FixedUpdate()
    {  
      
    }

    void LaunchRocket(GameObject hand)
    {
        GameObject projectile = Instantiate(rocket, 
            hand.transform.position , 
            hand.transform.rotation);
       
        
    }
    void OnTriggerEnter(Collider other)
    {
        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("objectInteractionTask"))
        {
            selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            // rotation: facing the user's entering direction
            float tempValueY = other.transform.position.y > 0 ? 12 : 0;
            Vector3 tmpTarget = new Vector3(hmd.transform.position.x, 
                tempValueY, hmd.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
            selectionTaskMeasure.taskStartPanel.SetActive(true);
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }

        
        // These are for the game mechanism.
    }
}