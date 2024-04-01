using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grab_with_prjectile : MonoBehaviour
{
    public GameObject owner;

    private Vector3 orgininal_position;

    private bool returning = false;

    public float speed = 1.0f;

    public GameObject selected;
    public bool something_selected = false;
    
    // Start is called before the first frame update
    void Start()
    {
       gameObject.transform.localScale /= 10;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                this.owner.transform.position, 0.7f);
            if (Vector3.Distance(transform.position,this.owner.transform.position) < 0.8f)
            {
                
                
                //+Debug.LogError("Is Returned ");
                
                returning = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                this.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void getSelected(GameObject thing)
    {
        Debug.LogWarning("Get Selected Called");
        if (selected == null)
        {
            
            thing =  new GameObject("Nothing");
        }
        else
        {
            thing =  selected;
            //Debug.LogError(thing);
        }
    }
    public void setOwner(GameObject hand)
    {
        this.owner = hand;
        gameObject.GetComponent<Rigidbody>().AddForce(hand.transform.forward * 30,ForceMode.Impulse);
    }

    void returnToOwner()
    {
        if (this.owner != null)
        {
            returning = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other == null)
        {
            selected = null;
            something_selected = false;
            returnToOwner();
        }
        else if (this.owner != null && other.gameObject != this.owner && !returning)
        {
            selected = other.gameObject;
            something_selected = true;
            
            returnToOwner();
        }
    }
}
