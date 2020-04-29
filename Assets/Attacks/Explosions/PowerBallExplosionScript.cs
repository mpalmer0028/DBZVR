using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallExplosionScript : MonoBehaviour
{
    public float growthRate;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(growthRate, growthRate, growthRate);
        if (!audioSource.isPlaying)
        {
            //Debug.Log("Not playing");
            Destroy(gameObject);
        }
    }
}
