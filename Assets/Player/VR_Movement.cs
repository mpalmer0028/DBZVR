﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VR_Movement : MonoBehaviour
{
    public SteamVR_Action_Vector2 leftStickInput;
    public SteamVR_Action_Boolean flyUpInput;
    public SteamVR_Action_Boolean flyDownInput;
    public float groundedDistance = 1;
    public float speed = 1;
    public float flySpeed = 10;
    public float flySpeedFactorBasedOnDistanceFromPlanet = 1;
    public GameObject planet;
    public GameObject leftHand;
    public GameObject rightHand;
    public PlayerGravityBody playerGravityBody;
    public AudioClip flySound;
    public AudioClip walkSound;

    private AudioSource audioSource;
    private CharacterController characterController;
    private bool flySoundPlaying = false;
    private bool walkSoundPlaying = false;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerGravityBody = GetComponent<PlayerGravityBody>();
        audioSource = GetComponent<AudioSource>();
        // Make the game run as fast as possible
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Vertical movement
        var gravity = flyUpInput.state ? flySpeed : (flyDownInput.state && !characterController.isGrounded ? 0 - flySpeed : 0);

        Vector3 gravityUp = (transform.position - planet.transform.position).normalized;
        Vector3 localUp = transform.up;

        if (leftStickInput.axis.magnitude > 0.2f)
        {
            var direction = Player.instance.hmdTransform.TransformDirection(new Vector3(leftStickInput.axis.x, 0, leftStickInput.axis.y));

            if (playerGravityBody.attractorPlanet)
            {
                var movement = speed * Time.deltaTime * (Vector3.Distance(this.transform.position, planet.transform.position) * flySpeedFactorBasedOnDistanceFromPlanet) *
                    (Vector3.ProjectOnPlane(direction, planet.transform.TransformDirection(transform.position)) +
                        (gravityUp * gravity / speed));
                // Move around planet
                MoveCharacter(movement);
                
                
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundedDistance))
                {
                    StartWalkingSoundIfNotPlaying();
                }
                else
                {
                    StartFlyingSoundIfNotPlaying();
                }
                
            }
            else
            {
                MoveCharacter(speed * Time.deltaTime *
                   (Vector3.ProjectOnPlane(direction, Vector3.up) +
                       (gravityUp * gravity / speed)
                   )
               );
                StartFlyingSoundIfNotPlaying();
            }
        }
        else
        {
            if (gravity != 0)
            {
                MoveCharacter(gravityUp * gravity * Time.deltaTime);
                StartFlyingSoundIfNotPlaying();
            }
            else
            {
                StopFlyingSound();
            }
        }
        
       

    }

    void StartFlyingSoundIfNotPlaying()
    {
        if (!flySoundPlaying)
        {
            walkSoundPlaying = false;
            audioSource.clip = flySound;
            audioSource.Play();
            flySoundPlaying = true;
        }
    }

    void StopFlyingSound()
    {
        audioSource.Pause();
        flySoundPlaying = false;
    }

    void StartWalkingSoundIfNotPlaying()
    {
        if (!walkSoundPlaying)
        {
            flySoundPlaying = false;
            audioSource.clip = walkSound;
            audioSource.Play();
            walkSoundPlaying = true;
        }
    }

    void StopWalkingSound()
    {
        audioSource.Pause();
        walkSoundPlaying = false;
    }
    CollisionFlags MoveCharacter(Vector3 motion)
    {
        //OpenVR.System.Get
        //OpenVR.System.ApplyTransform();
        return characterController.Move(motion);
    }
}
