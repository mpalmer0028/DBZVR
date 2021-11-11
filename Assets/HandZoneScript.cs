using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandZoneScript : MonoBehaviour
{
	public GameObject CloserZone;
	public GameObject OuterZone;
	public GameObject OverheadZone;
    
	public bool InCloserZone = false;
	public bool InOuterZone = false;
    public bool InOverheadZone = false;
	
	public float ExitCloserZoneTime;
	public float ExitOuterZoneTime;
	
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == OverheadZone.name)
        {
            InOverheadZone = true;
        }
        else if (collision.collider.name == OuterZone.name)
        {
            InOuterZone = true;
        }
        else if (collision.collider.name == CloserZone.name)
        {
            InCloserZone = true;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == OverheadZone.name)
        {
            InOverheadZone = false;
        }
        else if (collision.collider.name == OuterZone.name)
        {
	        InOuterZone = false;
	        ExitOuterZoneTime = Time.time;
        }
        else if (collision.collider.name == CloserZone.name)
        {
	        InCloserZone = false;
	        ExitCloserZoneTime = Time.time;
        }

    }
}
