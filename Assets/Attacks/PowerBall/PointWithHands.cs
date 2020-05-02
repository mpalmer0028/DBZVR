using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWithHands : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Lerp(leftHand.transform.rotation, rightHand.transform.rotation, 0.5f);
    }
}
