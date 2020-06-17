using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEmitterScript : MonoBehaviour
{
    public ParticleSystem beamSystem;
    private float life;
    private Vector3 initScale;
    private Vector3 step;

    

    // Start is called before the first frame update
    void Start()
    {
        if (beamSystem == null)
        {
            throw new Exception("No curve");
        }
        life = 150;
        initScale = transform.localScale;
        step = initScale / life;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(life.ToString() + " " + beamSystem.sizeOverLifetime.size.curve.Evaluate(life).ToString());
        transform.localScale -= step;
        life--;
        if (life < 0f)
        {            
            //Debug.Log("See Ya");
            Destroy(gameObject);
        }
    }
}
