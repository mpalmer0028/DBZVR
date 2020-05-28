using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour, IDestructible
{
    public GameObject Crate;
    public GameObject Pieces;

    public GameObject Destruct()
    {
        var pos = Crate.transform.position;
        var rot = Crate.transform.rotation;
        var objGrav = Crate.GetComponent<ObjectGravity>();
        var planet = objGrav.planet;
        var grav = objGrav.gravity;
        Destroy(Crate);
        var retval = Instantiate(Pieces, pos, rot, this.transform);
        //for (var i = 0; i < retval.transform.childCount; i++)
        //{
        //    var c = transform.GetChild(i);
        //    var cObjGrav = c.GetComponent<ObjectGravity>();
        //    cObjGrav.planet = planet;
        //    cObjGrav.gravity = grav;
        //}

        foreach(var cObjGrav in retval.transform.GetComponentsInChildren<ObjectGravity>())
        {
            //Debug.Log(planet.name);
            cObjGrav.planet = planet;
            cObjGrav.gravity = grav;
        }
        return retval;      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
