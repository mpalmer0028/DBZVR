using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PowerVolumeScript : MonoBehaviour
{
	public GameObject SpikesParent;
	public GameObject Player;
	private GameObject[] Spikes;
	
    // Start is called before the first frame update
    void Start()
	{
		var spikes = new List<GameObject>();
		foreach(Transform child in SpikesParent.transform){
			if(child != SpikesParent.transform){
				spikes.Add(child.gameObject);
			}
		}
		Spikes = spikes.ToArray();
		//Debug.Log(Spikes.Length);
    }

    // Update is called once per frame
    void Update()
	{
		transform.rotation = Player.transform.rotation;
		var randomIndexes = new List<int>();
		while(randomIndexes.Count < 20){
			int r = Random.Range(0, Spikes.Length-1);
			if(!randomIndexes.Contains(r)){
				Spikes[r].SetActive(!(Spikes[r].active));
				randomIndexes.Add(r);
			}
			
		}
						
    }
}
