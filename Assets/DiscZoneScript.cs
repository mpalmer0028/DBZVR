using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscZoneScript : MonoBehaviour
{
    public bool InTheZone = false;

    void OnCollisionEnter(Collision collision)
    {
        InTheZone = true;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        InTheZone = false;
    }
}
