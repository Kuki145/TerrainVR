using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainDeformer : MonoBehaviour {
	Terrain terrain;
	// Use this for initialization
	void Start () {
		terrain = GetComponent<Terrain> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
