using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DiskScript : MonoBehaviour
{
    public AudioClip loopClip;
    public AudioClip fireClip;

    private bool fired = false;
    private int maxSize = 8;
    private AudioSource audioSource;
    private Rigidbody rb;

    public float snapSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Player.instance.transform.rotation;
        if (fired)
        {
            //transform.position += transform.rotation * new Vector3(.1f, 0, 0);
            rb.AddForce(Player.instance.hmdTransform.transform.rotation * new Vector3(0, 0, 50));
            //transform.LookAt(Player.instance.hmdTransform.transform);
            transform.rotation = Player.instance.hmdTransform.transform.rotation;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Player.instance.hmdTransform.transform.rotation, snapSpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.localScale.x < maxSize)
            {
                var rate = .1f;
                transform.localScale += new Vector3(rate,rate,rate);
            }

            if (!audioSource.isPlaying)
            {
                audioSource.clip = loopClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    public void Fire()
    {
        //Player.instance.hmdTransform.transform.rotation;
        fired = true;
        this.transform.parent = null;
        audioSource.clip = fireClip;
        audioSource.loop = false;
        audioSource.Play();

        rb.constraints = RigidbodyConstraints.None;
    }
}
