using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceScript : MonoBehaviour
{
    public SkyboxScript sbs;
    public AudioClip clip;
    public AudioSource audioSource;

    

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        var des = collider.GetComponent<IDestructible>();
        if (des == null)
        {
            des = collider.transform.parent.gameObject.GetComponent<IDestructible>();
        }
        if (des != null)
        {
            des.Destruct();
            Destroy(GetComponent<BoxCollider>());
            sbs.Blackout(clip.length/2);
            audioSource.PlayOneShot(clip);
            Destroy(gameObject, clip.length);
        }        
    }
}
