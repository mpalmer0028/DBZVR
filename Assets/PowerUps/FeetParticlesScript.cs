using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FeetParticlesScript : MonoBehaviour
{
    public YellCatcher yellCatcher;
    public float factor;

    private Transform playerTranform;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    // Start is called before the first frame update
    void Start()
    {
        playerTranform = Player.instance.transform;
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerTranform.rotation;
        em.rateOverTime = yellCatcher.micLoudness * factor;
    }
}
