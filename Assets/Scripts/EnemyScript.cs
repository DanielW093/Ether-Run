﻿using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	Animator anim;
	public int rotateSpeed = 10;
	public int aggroDistance = 15;

	private double health = 1.5;

	public float normalSpeed;
	public float inBeamSpeed;
	public float focusedSpeed;

	public GameObject deathParticle;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		anim.SetFloat ("WalkSpeed", normalSpeed);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (PlayerScript.gameRunning) {
			//Rotate torch towards mouse
			Vector3 playerPos = GameObject.FindGameObjectWithTag ("Player").gameObject.transform.position;
			if (playerPos.x < this.gameObject.transform.position.x) {
				this.transform.eulerAngles = new Vector3 (0, 270, 0);
			} else {
				this.transform.eulerAngles = new Vector3 (0, 90, 0);
			}
			//Control Enemy Aggro
			float dist = Vector3.Distance (playerPos, this.gameObject.transform.position);

			if (dist <= aggroDistance) {
				anim.SetBool ("Walk", true);
				anim.SetBool ("Idle", false);
			} else {
				
				anim.SetBool ("Walk", false);
				anim.SetBool ("Idle", true);
			}

			Vector3 reAlign = transform.position;
			reAlign.z = 0f;
			transform.position = reAlign;

			if (health <= 0)
				Destroy ();
		}
	}

	public void Destroy()
	{
		GameObject.Find ("deathNoise").GetComponent<AudioSource> ().Play ();
		Instantiate (deathParticle, transform.position, transform.rotation);
		Destroy (transform.gameObject);
	}

	void OnTriggerStay(Collider c)
	{
		if(c.CompareTag ("Beam"))
		{
			if(c.gameObject.GetComponentInParent<PlayerScript>().isFocusing == true)
			{
				anim.SetFloat ("WalkSpeed", focusedSpeed);
				health-= 1*Time.deltaTime;
			}
			else{
				anim.SetFloat ("WalkSpeed", inBeamSpeed);
			}
		}
	}

	void OnTriggerExit(Collider c)
	{
		if (c.CompareTag("Beam")) {
			anim.SetFloat ("WalkSpeed", normalSpeed);
		}
	}
}
