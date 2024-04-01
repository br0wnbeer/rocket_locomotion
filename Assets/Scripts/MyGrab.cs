using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyGrab : MonoBehaviour
{
    public OVRInput.Controller controller;
    [SerializeField] public GameObject right_controller;
    private float triggerValue;
    private bool isInCollider;
    private bool isSelected;
    [SerializeField] public GameObject hmd;
    [SerializeField] public GameObject rocket;
    public GameObject selectedObj;
    public SelectionTaskMeasure selectionTaskMeasure;
    private float NextFire = 0;
    private GameObject current_rocket = null;
    private bool start = false;
    private bool done = false;
    private bool stationary = false;

    private void Start()
    {
        selectedObj = gameObject;
        
    }

    void Update()

    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        if (triggerValue > 0.95 && (Time.time > NextFire))
        {
            NextFire = Time.time + .8f;
            shoot_projectile();
        }

        if (OVRInput.Get(OVRInput.Button.Three))
        {
            stationary = !stationary;
        }

        if (current_rocket != null)
        {
            if (current_rocket.GetComponentInChildren<grab_with_prjectile>().something_selected )
            {
                Debug.LogWarning("Current : " +current_rocket.GetComponentInChildren<grab_with_prjectile>().selected);
                selectedObj = current_rocket.GetComponentInChildren<grab_with_prjectile>().selected;
                if (selectedObj.gameObject.CompareTag("objectT"))
                {
                    isInCollider = true;
                    if (!stationary)
                    {
                        selectedObj.transform.position = Vector3.MoveTowards(selectedObj.transform.position,
                            right_controller.transform.position, 0.3f);
                        
                        Quaternion rot = right_controller.transform.rotation;
                        selectedObj.transform.rotation = rot;
                    }
                    else if (stationary)
                    {
                        //translation with left thumb stick and right controller input forward 
                        float input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)[0];
                        if (input != 0)
                            selectedObj.transform.position += right_controller.transform.forward * input * 0.02f;
                        isInCollider = false;

                    }
                    else
                    {
                        isInCollider = false; }
                    
                }
                else if (selectedObj.gameObject.CompareTag("selectionTaskStart"))
                {
                    selectionTaskMeasure.isTaskStart = true;
                    if (!start)
                    {
                        
                        start = true;
                        if (!selectionTaskMeasure.isCountdown)
                        {
                            selectionTaskMeasure.isTaskStart = true;
                            selectionTaskMeasure.StartOneTask();
                        }
                    }
                }
                else if (selectedObj.gameObject.CompareTag("done"))

                {       Destroy(current_rocket);
                        done = false;
                        start = false;
                        stationary = false;
                        selectionTaskMeasure.isTaskStart = false;
                        
                        selectionTaskMeasure.isTaskStart = false;
                        selectionTaskMeasure.EndOneTask(); 
                        selectedObj = null;
                    
                }
            }
             
        }
    }

    private void shoot_projectile()
    {
        
        GameObject p = Instantiate(rocket, transform.position, transform.rotation);
        p.GetComponentInChildren<grab_with_prjectile>().setOwner(this.gameObject);
        current_rocket = p;
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = true;
            selectedObj = other.gameObject;
        }
        else if (other.gameObject.CompareTag("selectionTaskStart"))
        {
            if (!selectionTaskMeasure.isCountdown)
            {
                selectionTaskMeasure.isTaskStart = true;
                selectionTaskMeasure.StartOneTask();
            }
        }
        else if (other.gameObject.CompareTag("done"))
        {
            selectionTaskMeasure.isTaskStart = false;
            selectionTaskMeasure.EndOneTask();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = false;
            selectedObj = null;
        }
    }
}