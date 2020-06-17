using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBombExplodeScript : MonoBehaviour
{
    public GameObject explosion;
    private Vector3 initFirePos;

    void Start()
    {
        initFirePos = transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (Vector3.Distance(collision.GetContact(0).point, initFirePos) > transform.localScale.x/3)
        {
            //Debug.Log(Vector3.Distance(this.transform.position, initFirePos));
            var des = collision.gameObject.GetComponent<IDestructible>();
            if (des == null)
            {
                des = collision.gameObject.transform.parent.gameObject.GetComponent<IDestructible>();
            }
            if (des != null)
            {
                des.Destruct();
            }
            Destroy(this.gameObject, 2);
        }
    }

    public void OnDestroy()
    {
        var exp = Instantiate(explosion, transform.position, UnityEngine.Random.rotation);
        exp.transform.parent = null;
        exp.transform.localScale = transform.localScale/10;
    }

}
