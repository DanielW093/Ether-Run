using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

	private ParticleSystem particle;
	float startTime;

	// Use this for initialization
	void Start () {
		particle = this.gameObject.GetComponentInParent<ParticleSystem> ();
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (particle.duration <= Time.time - startTime)
			Destroy (gameObject);
	}
}
