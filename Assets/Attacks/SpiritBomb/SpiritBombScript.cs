using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpiritBombScript : MonoBehaviour
{
    public GameObject Ball;
    public float MaxSize;
    public float Rate;
    public ParticleSystem fingers_ps;
    public ParticleSystem charge_ps;

    public AudioClip startAudio;
    public AudioClip endAudio;
    public AudioClip loopAudio;
    public float speed = .5f;

    private AudioSource audioSource;
    private Rigidbody rb;
    private Transform ballTransform;
    private ParticleSystem.ShapeModule fingerShape;
    private Transform playerTransform;
    private Transform headsetTransform;
    private Transform fingerTransform;
    private Transform chargeTransform;
    private Plane headsetPlane;
    private Vector3 startpos;
    private Ray ray;
    private float enter;
    private ParticleSystem.Particle[] particles;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerTransform = Player.instance.transform;
        headsetTransform = Player.instance.hmdTransform.transform;
        fingerTransform = fingers_ps.transform;
        chargeTransform = charge_ps.transform;
        ballTransform = Ball.transform;
        fingerShape = fingers_ps.shape;
        startpos = headsetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        fingerTransform.LookAt(playerTransform.position, playerTransform.up);
        //chargeTransform.LookAt(playerTransform.position, playerTransform.up);
        
        //var ls = ballTransform.localScale.magnitude;
        //Debug.Log(ballTransform.localScale.magnitude);
        //Debug.Log(transform.parent);

        if (MaxSize > ballTransform.localScale.x && transform.parent != null)
        {
            chargeTransform.LookAt(playerTransform.position, playerTransform.up);
            var rateVector = new Vector3(Rate, Rate, Rate);
            //transform.rotation = playerTransform.rotation;
            ballTransform.localScale += rateVector;
            //ballTransform.lo += rateVector;
            
            startpos += new Vector3(0, Rate, 0);
            //fingerShape.scale += rateVector;
            fingerShape.radius = ballTransform.localScale.x;
            //Debug.Assert(ls < ballTransform.localScale.magnitude);
            //fingerShape.position = new Vector3(0, Rate / 10, 0);
            //ballTransform.;
        }

        //var c = charge_ps.GetParticles(particles);
        
        if (transform.parent == null)
        {
            rb.AddForce(Player.instance.hmdTransform.forward * speed * ballTransform.localScale.x);
            
            if (audioSource.time > audioSource.clip.length / 2)
            {
                //ballTransform.rotation = Player.instance.hmdTransform.rotation;
            }
            
        }
        else
        {
            if (audioSource.clip == startAudio && !audioSource.isPlaying)
            {
                audioSource.clip = loopAudio;
                audioSource.loop = true;
                audioSource.Play();
            }
            //for(var i = 0;i<particles.Length;i++)
            //{
            //    particles[i].position = Vector3.Lerp(ballTransform.position, particles[i].position, .1f);
            //    charge_ps.GetParticles(particles);
            //}
            //charge_ps.SetParticles(particles,c);
        }
    }

    public void Fire()
    {
        audioSource.clip = endAudio;
        audioSource.loop = false;
        audioSource.Play();
        transform.parent = null;
        var em = charge_ps.emission;
        em.rateOverTime = 0;
        charge_ps.SetParticles(new ParticleSystem.Particle[] { });
        
        Destroy(gameObject, endAudio.length + 5);
    }
}
