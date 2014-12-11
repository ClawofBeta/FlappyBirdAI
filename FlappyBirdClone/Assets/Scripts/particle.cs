using UnityEngine;
using System.Collections;

public class particle : MonoBehaviour {
	public ParticleSystem ps;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!ps.IsAlive()){
			Destroy (this.gameObject);
		}
	}
}
