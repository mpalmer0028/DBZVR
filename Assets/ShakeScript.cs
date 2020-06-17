using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScript : MonoBehaviour, IDestructible
{

    public GameObject Pieces;

    // Start is called before the first frame update
    void Start()
    {
        //Destruct();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Destruct()
    {
        var pos = transform.position;
        var rot = transform.rotation;
        Destroy(this.gameObject, 0.01f);
        return Instantiate(Pieces, Vector3.zero, rot);
    }
}
