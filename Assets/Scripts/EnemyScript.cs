using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	int type;
	Animator anim;
	public int rotateSpeed = 10;
	public int aggroDistance = 15;

	private double health = 10;

	private float moveSpeed;

	public float normalSpeed;
	public float inBeamSpeed;
	public float focusedSpeed;

	public GameObject deathParticle;

	// Use this for initialization
	void Start () {
		if(transform.Find("BadGuyWalking") != null)
		{
			type = 0;
			anim = transform.Find("BadGuyWalking").GetComponent<Animator> ();
		}
		else
		{
			type = 1;
			anim = transform.Find("SpiderWalkCycle").GetComponent<Animator>();
		}

		anim.speed = normalSpeed;
		moveSpeed = normalSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PlayerScript.gameRunning && !anim.enabled)
			anim.enabled = true;

		if(!PlayerScript.gameRunning && anim.enabled)
			anim.enabled = false;

		if (PlayerScript.gameRunning) {
			//Rotate enemy towards player
			Vector3 playerPos = GameObject.FindGameObjectWithTag ("Player").gameObject.transform.position;
			if (playerPos.x < this.gameObject.transform.position.x) {
				this.transform.eulerAngles = new Vector3 (0, 270, 0);
			} else {
				this.transform.eulerAngles = new Vector3 (0, 90, 0);
			}
			//Control Enemy Aggro
			float dist = Vector3.Distance (playerPos, this.gameObject.transform.position);

			if (dist <= aggroDistance) {
				Vector3 newPos = transform.position;

				newPos += moveSpeed * transform.forward * Time.deltaTime;

				transform.position = newPos;
			} 

			Vector3 reAlign = transform.position;
			reAlign.z = 0f;
			transform.position = reAlign;

			if (health <= 0)
				Kill ();
		}
	}

	public void Kill()
	{
		GameObject.Find ("deathNoise").GetComponent<AudioSource> ().Play ();
		Vector3 partPos = transform.position; partPos.y += 0.5f;
		Instantiate (deathParticle, partPos, transform.rotation);
		Destroy (transform.gameObject);
	}

	private void Destroy()
	{
		Destroy (transform.gameObject);
	}

	void OnTriggerStay(Collider c)
	{
		if(c.CompareTag ("Beam")) //If the trigger is torch beam
		{
			if(c.gameObject.GetComponentInParent<PlayerScript>().isFocusing == true) //If beam is focused
			{
				anim.speed = focusedSpeed; //Slow down
				moveSpeed = focusedSpeed;
				health-= 16*Time.deltaTime; //Take away health
			}
			else{
				anim.speed = inBeamSpeed; //Slow down a bit
				moveSpeed = inBeamSpeed;
			}
		}
			
		if(c.gameObject.CompareTag("Enemy"))
		{
			Destroy();
			Debug.Log("ENEMY INSIDE ENEMY, DESTROYING");
		}

		if(type == 1)
		{
			if(c.gameObject.name == "WallDoor")
			{
				Destroy();
				Debug.Log("CEILING ENEMY HIT DOOR, DESTROYING");
			}
		}
	}

	void OnTriggerExit(Collider c)
	{
		if (c.CompareTag("Beam")) { //If the trigger is the beam
			anim.speed = normalSpeed; //Return speed to normal
			moveSpeed = normalSpeed;
		}
	}
}
