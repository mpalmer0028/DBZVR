using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    public float gravity = -12;
    public GameObject planet;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb != null && planet != null)
        {
            var force = (planet.transform.position - transform.position) * gravity * -1;
            //Debug.Log(Quaternion.);
            //Debug.Log(transform.position);
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, force, out hit, .2f))
            {
                rb.AddForce(force);
            }
            
        }
    }
}
