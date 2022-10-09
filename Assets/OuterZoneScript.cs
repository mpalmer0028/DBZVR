using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class OuterZoneScript : MonoBehaviour
{
	public Transform handTranform;

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(handTranform);
		//Debug.Log(transform.rotation);
	}
}

