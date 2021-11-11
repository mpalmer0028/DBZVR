using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpawner : MonoBehaviour
{
	public float TimeFromCloserToOuter =.03f;
	public bool IsPunching{
		get{
			if(audioSource){
				return audioSource.isPlaying;
			}
			return false;
		}
		set{
			if(!audioSource.isPlaying && value){
				audioSource.PlayOneShot(SwingClip);
				Swung = true;
			}
		}
	}
    public AudioClip SwingClip;
    private AudioSource audioSource;
	private HandZoneScript handZoneScript;
	private bool Swung;

    // Start is called before the first frame update
    void Start()
	{
		//TimeFromCloserToOuter = audioSource.clip.length;
        audioSource = GetComponent<AudioSource>();
        handZoneScript = GetComponent<HandZoneScript>();
    }

    // Update is called once per frame
    void Update()
    {        
	    if (!IsPunching && handZoneScript.InOuterZone && 
		    handZoneScript.ExitCloserZoneTime + TimeFromCloserToOuter >= Time.time &&
	    	!Swung)
        {
		    IsPunching = true;
        } else if(handZoneScript.InCloserZone){
        	Swung = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("hit "+ other.gameObject.name);
    //    if (IsPunching)
    //    {
            
    //    }
    //    var des = other.gameObject.GetComponent<IDestructible>();
    //    if (des == null)
    //    {
    //        des = other.gameObject.transform.parent.gameObject.GetComponent<IDestructible>();
    //    }
    //    if (des != null)
    //    {
    //        des.Destruct();
    //    }
    //}

    //public GameObject Punch()
    //{
    //    IsPunching = true;
    //    audioSource.PlayOneShot(SwingClip);
    //    return this.gameObject;
    //}

      
}
