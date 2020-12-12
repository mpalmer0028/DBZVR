using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FresnelSlideScript : MonoBehaviour
{
	public float InitialFresnelPower = 0;
	public float EndFresnelPower = .75f;
	public float TimeToLerp = 5;
	
	private Renderer renderer;
	private float InitialTime;

    // Start is called before the first frame update
    void Start()
    {
	    renderer = GetComponent<Renderer>();
	    renderer.material.SetFloat("_FresnelPower", InitialFresnelPower);
	    Debug.Log(GetComponent<Renderer>().material.name);
	    Debug.Log(GetComponent<Renderer>().material.GetVector("FresnelPower"));
	    InitialTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {	    
	    renderer.material.SetFloat("_FresnelPower", Mathf.Lerp(InitialFresnelPower, EndFresnelPower, (Time.time - InitialTime) / TimeToLerp));
    }
}
