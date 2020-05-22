using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class OverHeadScript : MonoBehaviour
{
    private Transform playerTranform;
    // Start is called before the first frame update
    void Start()
    {
        playerTranform = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerTranform.rotation;
    }
}
