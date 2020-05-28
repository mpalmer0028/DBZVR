using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FeetParticlesScript : MonoBehaviour
{
    public YellCatcher yellCatcher;
    public float factor;
    public float rockFactor;
    public ParticleSystem rock0;
    public ParticleSystem rock1;
    public ParticleSystem rock2;
    public ParticleSystem rock3;
    public AudioClip powerUpTriggeredClip;
    public float powerUpTriggerVolume;
    public GameObject SuperHandGlowL;
    public GameObject SuperHandGlowR;

    private Transform playerTranform;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    private AudioSource audioSource;
    private AudioClip startClip;
    private bool poweredUp;
    private bool rocksStopped;

    // Start is called before the first frame update
    void Start()
    {
        playerTranform = Player.instance.transform;
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
        audioSource = GetComponent<AudioSource>();
        startClip = audioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(yellCatcher.micLoudness*1000);
        // If it is loud
        if(yellCatcher.micLoudness*1000 > powerUpTriggerVolume && !poweredUp)
        {
            //Audio
            if(audioSource.time == audioSource.clip.length)
            {
                audioSource.PlayOneShot(powerUpTriggeredClip);
                poweredUp = true;
            }
            if (!audioSource.isPlaying)
            {
                audioSource.time = 0;
                audioSource.Play();
            }

            //Particles
            transform.rotation = playerTranform.rotation;
            var rf = yellCatcher.micLoudness * rockFactor;
            em.rateOverTime = rf;
            foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
            {
                var rem = rock.emission;
                rem.rateOverTime = rf;
            }

        }
        else if (!poweredUp)
        {
            audioSource.Stop();            
        }

        if (poweredUp && !rocksStopped)
        {
            var rf = 0;
            em.rateOverTime = rf;

            foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
            {
                var rem = rock.emission;
                rem.rateOverTime = rf;
            }
            SuperHandGlowL.SetActive(true);
            SuperHandGlowR.SetActive(true);
            rocksStopped = true;
        }
    }
}
