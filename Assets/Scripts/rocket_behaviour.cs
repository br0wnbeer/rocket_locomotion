using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

//using UnityEngine.PlayerLoop.PreUpdate.PhysicsUpdate;
public class rocket_behaviour : MonoBehaviour
{
    [SerializeField] private float speed = 20.0f;
    [SerializeField]public float strength = 1000;
    [SerializeField] float radius = 20.0f;
    [SerializeField] public GameObject explosion;

    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
        transform.rotation = Quaternion.Euler(90,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
        foreach (Collider c in colliders)
        {
            Rigidbody rigidbody = c.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                
                rigidbody.AddExplosionForce(strength, transform.position, radius, 3.0f);
                Destroy(this.gameObject);
                var hit = Instantiate(explosion, transform.position, transform.rotation);
                AudioSource.PlayClipAtPoint(clip,transform.position);
                Destroy(hit , 2);
                
            }
        }
    }
}