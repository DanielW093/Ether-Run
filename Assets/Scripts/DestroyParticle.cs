using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

	private ParticleSystem particle;
	float startTime;

	// Use this for initialization
	void Start () {
		particle = this.gameObject.GetComponentInParent<ParticleSystem> (); //Get particle system
		startTime = Time.time; //Set start time
	}
	
	// Update is called once per frame
	void Update () {
		if (particle.duration <= Time.time - startTime) //If particle has carried out its duration
			Destroy (gameObject); //Destroy particle
	}
}
