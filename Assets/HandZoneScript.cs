using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandZoneScript : MonoBehaviour
{
    public GameObject OverheadZone;
    public GameObject OuterZone;

    public bool InOverheadZone = false;
	public bool InOuterZone = false;
	void Start(){
	}

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
        }

    }
}
