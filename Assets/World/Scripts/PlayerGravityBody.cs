using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerGravityBody : MonoBehaviour {

    public PlanetScript attractorPlanet;
    public SteamVR_Action_Boolean toggleSnapInput;

    private Transform playerTransform;
    private CharacterController cc;
    private PlanetScript storedPlanet;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        //cc.useGravity = false;
        //cc.constraints = RigidbodyConstraints.FreezeRotation;

        playerTransform = transform;
        storedPlanet = attractorPlanet;
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (toggleSnapInput.state)
        {
            if (attractorPlanet == null)
            {
                attractorPlanet = storedPlanet;
            }
            else
            {
                storedPlanet = attractorPlanet;
                attractorPlanet = null;
            }
        }

        if (attractorPlanet)
        {
            attractorPlanet.Attract(playerTransform, cc);
        }
    }
    void TogglePlanetGravity()
    {

    }
}
