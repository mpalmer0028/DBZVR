using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBeamSystemScript : MonoBehaviour
{
	public Transform Powerball;
	public Transform StartingTrasform;
	
	
	/// <summary>
	/// Number of verts for each ring
	/// </summary>
	private int RingResolution = 16;
	private float RingDistance = 1;
	
	private Mesh Mesh;
	
    // Start is called before the first frame update
    void Start()
    {
	    //GetComponent<>
	    this.Mesh = new Mesh();
	    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
