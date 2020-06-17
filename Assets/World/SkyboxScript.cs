using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    public Material blackMaterial;
    private Skybox skybox;
    private float blackoutTime;
    //private Time blackoutTime;

    // Start is called before the first frame update
    void Start()
    {
        skybox = GetComponent<Skybox>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(blackoutTime > Time.fixedTime)
        {
            skybox.material = blackMaterial;
        }
        else
        {
            skybox.material = null;
        }
    }

    public void Blackout(float time)
    {
        blackoutTime = Time.fixedTime + time;
    }
}
