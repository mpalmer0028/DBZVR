using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpawner : MonoBehaviour
{
    public bool IsPunching = false;
    public AudioClip SwingClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (IsPunching)
        {
            if (!audioSource.isPlaying)
            {
                IsPunching = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit "+ other.gameObject.name);
        if (IsPunching)
        {
            
        }
        var des = other.gameObject.GetComponent<IDestructible>();
        if (des == null)
        {
            des = other.gameObject.transform.parent.gameObject.GetComponent<IDestructible>();
        }
        if (des != null)
        {
            des.Destruct();
        }
    }

    public GameObject Punch()
    {
        IsPunching = true;
        audioSource.PlayOneShot(SwingClip);
        return this.gameObject;
    }

      
}
