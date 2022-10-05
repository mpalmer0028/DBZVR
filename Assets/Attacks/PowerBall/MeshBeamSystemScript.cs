using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MeshBeamSystemScript : MonoBehaviour
{
	public float Radius = 1;
	public float LOD_Divisions = 4;
	
	/// <summary>
	/// Object where beam comes from
	/// </summary>
	public GameObject EmittingObject;
	
	/// <summary>
	/// Object that beam folows
	/// </summary>
	public GameObject ObjectToFollow;

	
	/// <summary>
	/// Number of verts for each ring
	/// </summary>
	public int RingResolution = 16;
	public float RingDistance = 1;
	private float DrawDistance;
	
	
	
	private MeshFilter MF;
	private MeshRenderer MR;
	private List<BeamRing> rings;
	
    // Start is called before the first frame update
    void Start()
	{
		MF = GetComponent<MeshFilter>();
		MR = GetComponent<MeshRenderer>();
    	
	    var mesh = new Mesh();
		DrawDistance = Camera.current.farClipPlane;
	    
		mesh.vertices = new Vector3[0];
		mesh.uv = new Vector2[0];
	    
		AddRing(ref mesh, RingResolution);
	    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	private void AddRing(ref Mesh mesh, int ringVertCount){
		var ring = new BeamRing(ringVertCount,
			ObjectToFollow.transform.position - EmittingObject.transform.position, 
			ObjectToFollow.transform.rotation * Quaternion.Inverse(EmittingObject.transform.rotation)
		);
			
		// amount of rotation  needed per ring vertex
		float dtheta = (float)(2 * Math.PI / ringVertCount);
		// set starting point
		float theta = 0f;
		
		// Build ring
		var verts = new Vector3[ringVertCount];
		for(var i = 0; i < ringVertCount; i++){
			var x = ((float)Math.Cos(theta) * Radius);
			var y = (float)Math.Sin(theta) * Radius;			
			verts[i] = (ring.rotation * new Vector3(x,y,0)) + ring.centerPosition;						
	    	
			theta -= dtheta;
		}
		
		// Add vertices
		var newVertsStartIndex = mesh.vertices.Length;
		ring.verticesPositions = verts;
		mesh.vertices = mesh.vertices.Concat(ring.verticesPositions).ToArray();
		ring.verticesIndexesInMesh = Enumerable.Range(newVertsStartIndex, ringVertCount).ToArray();
		
		if(rings == null){
			// add tris
			var prevRingIndex = rings.Count - 1;
			if(rings[prevRingIndex].ringVertCount == ringVertCount){
				var tris = new List<int>();
				for(var i = 0;i < ringVertCount;i++){
					if(i<ringVertCount-1){
						tris.AddRange(new int[]{
							ring.verticesIndexesInMesh[i], ring.verticesIndexesInMesh[i+1],rings[prevRingIndex].verticesIndexesInMesh[i],
							rings[prevRingIndex].verticesIndexesInMesh[i],ring.verticesIndexesInMesh[i+1], rings[prevRingIndex].verticesIndexesInMesh[i+1]
						});
					} else {
						tris.AddRange(new int[]{
							ring.verticesIndexesInMesh[i], ring.verticesIndexesInMesh[0],rings[prevRingIndex].verticesIndexesInMesh[i],
							rings[prevRingIndex].verticesIndexesInMesh[i],ring.verticesIndexesInMesh[0], rings[prevRingIndex].verticesIndexesInMesh[0]
						});
					}
					
				}
				mesh.SetTriangles(tris,0);
			}
			rings.Add(ring);
		} else {
			rings = new List<BeamRing>{ring};
		}
		
		
		
	}
}

public struct BeamRing{
	public int ringVertCount;
	public int[] verticesIndexesInMesh;
	public Vector3[] verticesPositions;
	/// <summary>
	/// position from emitting object to followed object
	/// </summary>
	public Vector3 centerPosition;
	public Quaternion rotation;
	
	public BeamRing(int ringVertCount, Vector3 centerPosition, Quaternion rotation){
		this.ringVertCount = ringVertCount;
		this.verticesIndexesInMesh = null;
		this.verticesPositions = null;
		this.centerPosition = centerPosition;
		this.rotation = rotation;
		
	}
}
