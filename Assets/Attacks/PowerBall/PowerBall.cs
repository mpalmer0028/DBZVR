using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PowerBall : MonoBehaviour
{
    public int life;
    public float speed = .5f;
    public float maxBallScale = 3f;

    public int beamDelay = 60;
    public ParticleSystem beamSystem;
    public ParticleSystem chargeSystem;

    public GameObject blastEmitter;
    public GameObject explosion;

    public bool fired;
    public YellCatcher yellCatcher;

    public AudioClip startAudio;
    public AudioClip endAudio;
    public AudioClip loopAudio;
    
    private Rigidbody rb;
    private ParticleSystem ps;
    private AudioSource audioSource; 
    private int i = 0; 
    private GameObject spawner; 
    private float emissionRate;

    private float initRateOverDistance;
    private Vector3 initFirePos;

    

    // Start is called before the first frame update
    void Start()
    {
        //if (yellCatcher==null||
        //    rb==null||
        //    ps==null||
        //    this.transform.parent == null
        //    )
        //{
        //    throw new Exception();
        //}
        spawner = transform.parent.gameObject;
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
                initFirePos = pos;
                var bes = blastEmitter.GetComponent<BlastEmitterScript>();
                bes.beamSystem = beamSystem;
                var emitter = Instantiate(blastEmitter, transform.position, transform.rotation, spawner.transform);
                Destroy(chargeSystem.gameObject);

            }
            
            if (audioSource.isPlaying)
            {
                rb.AddForce(transform.forward * speed);
                rb.rotation = Player.instance.hmdTransform.transform.rotation;
                i++;
            }
            else
            {
                //audioSource.Pause();
                Destroy(gameObject);
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

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (Vector3.Distance(collision.GetContact(0).point, initFirePos)> 10)
        {
            //Debug.Log(Vector3.Distance(this.transform.position, initFirePos));
            Destroy(this.gameObject);
        }
    }

    public void OnDestroy()
    {
        var exp = Instantiate(explosion, transform.position, UnityEngine.Random.rotation);
        exp.transform.parent = null;
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
    }
}
