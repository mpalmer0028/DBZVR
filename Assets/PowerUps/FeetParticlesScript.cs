using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FeetParticlesScript : MonoBehaviour
{
    public YellCatcher yellCatcher;
	public float micSensitivity = 500;
    public float rockFactor;
    public ParticleSystem rock0;
    public ParticleSystem rock1;
    public ParticleSystem rock2;
    public ParticleSystem rock3;
    public AudioClip powerUpTriggeredClip;
    public float powerUpTriggerVolume;
    public GameObject SuperHandGlowL;
    public GameObject SuperHandGlowR;
    public GameObject PowerVolume;

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
	    //Debug.Log(yellCatcher.Squeeze.axis);
        // If it is loud
	    if((yellCatcher.micLoudness*micSensitivity > powerUpTriggerVolume || yellCatcher.Squeeze.axis > .5f) && !poweredUp)
        {
            //Audio
            if(audioSource.time == audioSource.clip.length)
            {
                audioSource.PlayOneShot(powerUpTriggeredClip);
	            poweredUp = true;
	            PowerVolume.active = true;
	            
            }
            if (!audioSource.isPlaying)
            {
                audioSource.time = 0;
                audioSource.Play();
            }

            //Particles
            transform.rotation = playerTranform.rotation;
		    var rf = (yellCatcher.micLoudness + yellCatcher.Squeeze.axis)* rockFactor;
		    em.rateOverTime = rf;
		    var rockSpeed = 0.2f + (audioSource.time/audioSource.clip.length);
		    
            foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
            {
	            var rem = rock.emission;
	            rem.rateOverTime = rf;
	            var velocityOverTime = rock.velocityOverLifetime;
	            velocityOverTime.y = rockSpeed;
            }

        }
        else if (!poweredUp)
        {
	        if(audioSource.isPlaying){
	        	var rf = 0;
		        em.rateOverTime = rf;

		        foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
		        {
			        var rem = rock.emission;
			        rem.rateOverTime = 0;
			        var velocityOverTime = rock.velocityOverLifetime;
			        velocityOverTime.y = -4;
			        //Debug.Assert(rock.emission.rateOverTime.constant == 0);
		        }
		        audioSource.Stop(); 
	        }
	           
	        
	        
        }else if (poweredUp){
        	var rf = (yellCatcher.micLoudness + yellCatcher.Squeeze.axis)* rockFactor;
        	var inversePlayAmount = 1-(audioSource.clip.length/audioSource.time);
        	
	        em.rateOverTime = 0;
	        var rockSpeed = 2f*inversePlayAmount;
		    
	        foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
	        {
		        var rem = rock.emission;
		        rem.rateOverTime = rockFactor;
		        var velocityOverTime = rock.velocityOverLifetime;
		        velocityOverTime.y = .1f;
	        }
        }
	    if (PowerVolume.active && !audioSource.isPlaying){
        	PowerVolume.active = false;
        	poweredUp = false;
		    var rf = 0;
		    em.rateOverTime = rf;

		    foreach (var rock in new ParticleSystem[] { rock0, rock1, rock2, rock3 })
		    {
			    var rem = rock.emission;
			    rem.rateOverTime = 0;
			    var velocityOverTime = rock.velocityOverLifetime;
			    velocityOverTime.y = -4;
			    //Debug.Assert(rock.emission.rateOverTime.constant == 0);
		    }
            
        }

        
    }
}
