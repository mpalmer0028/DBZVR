using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBall : MonoBehaviour
{
    public int life;
    public int speed = 500;
    public float maxBallScale = 3f;

    public int beamDelay = 60;
    public ParticleSystem beamSystem;

    public bool fired;
    public YellCatcher yellCatcher;

    public AudioClip startAudio;
    public AudioClip endAudio;
    public AudioClip loopAudio;
    
    private Rigidbody rb;
    private ParticleSystem ps;
    private AudioSource audioSource; 
    private int i = 0; 
    private GameObject spawer; 
    private float emissionRate;

    private float initRateOverDistance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();

        emissionRate = ps.emission.rateOverTime.constant;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = startAudio;
        audioSource.Play();

        var beamEmission = beamSystem.emission;
        initRateOverDistance = beamEmission.rateOverDistance.constant;
        var beamRateOverDistance = beamEmission.rateOverDistance;
        beamRateOverDistance.constant = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.fired)
        {
            if (audioSource.clip == loopAudio || audioSource.clip == startAudio)
            {
                audioSource.clip = endAudio;
                audioSource.loop = false;
                audioSource.maxDistance = 1000;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.Play();
                var pos = this.transform.parent.position;
                var rot = this.transform.parent.localRotation;
                this.transform.parent = null;
                this.transform.position = pos;
                this.transform.localRotation = rot;

                //this.transform.localScale = this.transform.localScale;
            }
            
            if (life > i)
            {
                rb.AddForce(transform.forward * 50);
                i++;
            }
            else
            {
                audioSource.Pause();
                Destroy(this);
            }
        }
        else
        {
            if(audioSource.clip == startAudio && !audioSource.isPlaying)
            {
                audioSource.clip = loopAudio;
                audioSource.Play();
                audioSource.loop = true;
            }
        }
        var em = ps.emission.rateOverTime;
        em.mode = ParticleSystemCurveMode.Constant;
        if (transform.localScale.y < maxBallScale)
        {
            var micLoudness = yellCatcher.micLoudness * yellCatcher.factor/5;
            var growthRate = .1f * (micLoudness > 1 ? micLoudness : 1);
            transform.localScale += new Vector3(growthRate, growthRate, growthRate);
            if(micLoudness > 1)
            {
                em.constant = micLoudness + emissionRate;
            }
            else
            {
                em.constant = emissionRate;
            }

        }        
        
    }


    public void FixedUpdate()
    {
        if (this.fired)
        {
            if(beamDelay > 0)
            {
                beamDelay--;
            }
            else if(beamDelay == 0)
            {
                beamDelay--;
                var em = beamSystem.emission;
                    em.rateOverDistance = initRateOverDistance;
            }            
        }
    }

    public void Fire()
    {
        this.fired = true;
        Destroy(this.gameObject, life);
    }
}
