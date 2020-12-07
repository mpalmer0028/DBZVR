using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRodScript : MonoBehaviour
{
	public GameObject[] Rods;
	public int[] RodStartFrames;
	private float StartTime;
	private int Frame;
	
	void Start(){
		StartTime = Time.time;
	}

    // Update is called once per frame
    void Update()
	{
		var t = Time.time - StartTime;
		transform.localRotation *= Quaternion.Euler(0, 0, 1+(t/4));
	    for(var i = 0; i < Rods.Length; i++){
	    	if((!Rods[i].active) && RodStartFrames[i] <= t){
	    		//Debug.Log(Time.time -StartTime);
	    		Rods[i].SetActive(true);
	    	}
	    }
    }
}
