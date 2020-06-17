using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public bool local;
    
    // Update is called once per frame
    void Update()
    {
        var m = Quaternion.Euler(x, y, z);
        if (local)
        {
            transform.localRotation *= m;
        }
        else
        {
            transform.rotation *= m;
        }
    }
}
