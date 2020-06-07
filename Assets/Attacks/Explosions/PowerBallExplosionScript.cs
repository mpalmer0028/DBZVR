using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallExplosionScript : MonoBehaviour
{
    public float growthRate;
    public List<AudioClip> clips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        var i = Mathf.RoundToInt(((float)clips.Count * Random.value));
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[i];
        audioSource.Play();
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
