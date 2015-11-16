using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public static bool gameRunning = true;

	public float speed;
	CharacterController controller;

	const float gravity = 9.8f;
	const float jumpSpeed = 3;
	private float vSpeed = 0; //Vertical speed

	private Vector3 tTarget;
	const float rotateSpeed = 10;
	public GameObject torch;

	private int shardsCollected = 0;
	private int health = 10;
	private double battery = 100;

	public GameObject scoreText;
	public GameObject FloorText;
	public GameObject ShardText;
	public GameObject MirrorText;
	public GameObject BatteryText;
	public GameObject HealthText;
	private string[] numWords = new string[]{"null", "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth"};

	public GameObject loseText;
	public GameObject winText;
	public GameObject winText1;

	public bool isFocusing;

	private Light spotlight;
	const float dimInt = 0.6f;
	const float focInt = 1.2f;

	public Material dimMat;
	public Material brightMat;

	private GameObject[] enemies = new GameObject[5];
	public GameObject ethereal;

	public AudioSource torchClick;
	public AudioSource torchHum;
	public AudioSource pickup;

	// Use this for initialization
	void Start () {
		controller = this.GetComponent<CharacterController> ();
		isFocusing = false;

		spotlight = gameObject.GetComponentInChildren<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		int currentFloor = (int)(transform.position.y / 2);
		string score = "Shards: " + shardsCollected + "/10";
		string floor = " ";
		if(currentFloor < 9 && currentFloor > 0)
			floor = numWords[currentFloor] + " floor\nUse 'W' or 'S' to change floor";
		else if(currentFloor == 9)
			floor = "Top Floor\nUse 'S' to change floor";
		else if(currentFloor == 0)
			floor = "Ground Floor\nUse 'W' to change floor";
		scoreText.GetComponent<Text> ().text = score;
		FloorText.GetComponent<Text> ().text = floor;
		BatteryText.GetComponent<Text> ().text = "Battery: " + (int)battery + "%";
		HealthText.GetComponent<Text> ().text = "Health: " + health + "/10";

		if (gameRunning) {
			SpawnEnemies ();

			//Calculate Velocity
			Vector3 velocity = transform.right * Input.GetAxis ("Horizontal") * speed;
			//Gravity and Jumping
			if (controller.isGrounded) {
				vSpeed = 0;
			
				if (Input.GetAxis ("Jump") == 1) {
					vSpeed = jumpSpeed;
				}
			}
			vSpeed -= gravity * Time.deltaTime;
			velocity.y = vSpeed;
			//Move Player
			controller.Move (velocity * Time.deltaTime);

			//Rotate torch towards mouse
			Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
			tTarget = Camera.main.ScreenToWorldPoint (mousePos);
			tTarget.z = torch.transform.position.z;
			Vector3 targetDir = tTarget - torch.transform.position;
			float step = rotateSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (torch.transform.forward, targetDir, step, 0.0F);
			torch.transform.rotation = Quaternion.LookRotation (newDir);


			if (Input.GetMouseButtonDown (0) && battery > 0) {
				torchClick.Play ();
				torchHum.Play ();
				isFocusing = true;
			}

			if (Input.GetMouseButtonUp (0) || battery <= 0) {
				torchClick.Play ();
				torchHum.Stop ();
				isFocusing = false;
			}

			if (isFocusing && battery > 0) {
				spotlight.intensity = focInt;
				GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = brightMat;
				battery -= 4 * Time.deltaTime;
			}

			if (!isFocusing && battery < 100) {
				//Battery recharges when not being focused, and player is moving
				spotlight.intensity = dimInt;
				GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = dimMat;
				if (Input.GetAxis ("Horizontal") != 0)
					battery += 2 * Time.deltaTime;
			}

			if (health == 0) {
				gameRunning = false;
				loseText.GetComponent<Text>().enabled = true;
			}
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag ("Enemy"))
		{
			c.gameObject.GetComponent<EnemyScript>().Destroy ();
			health--;
			Debug.Log (health);
		}
	}

	void OnTriggerStay(Collider c)
	{
		if (c.CompareTag ("Collectible")) {
			ShardText.GetComponent<Text> ().enabled = true;
			if(Input.GetAxis ("Collect") > 0)
			{
				pickup.Play ();
				shardsCollected++;
				Destroy (c.gameObject);
				ShardText.GetComponent<Text> ().enabled = false;
			}
		}
		if (c.CompareTag ("LiftTrigger")) {
			FloorText.GetComponent<Text> ().enabled = true;
			if(Input.GetKeyDown("w") && c.gameObject.GetComponentInParent<RoomData>().canGoUp == true)
			{
				Vector3 newPos = transform.position;
				newPos.y += 2;
				transform.position = newPos;
				for(int i = 0; i < enemies.Length; i++)
				{
					Destroy (enemies[i]);
				}
			}
			else if(Input.GetKeyDown("s") && c.gameObject.GetComponentInParent<RoomData>().canGoDown == true )
			{
				Vector3 newPos = transform.position;
				newPos.y -= 2;
				transform.position = newPos;
				for(int i = 0; i < enemies.Length; i++)
				{
					Destroy (enemies[i]);
				}
			}
		}
		if (c.CompareTag ("Mirror")) {
			MirrorText.GetComponent<Text>().enabled = true;
			if(shardsCollected == 10)
			{
				MirrorText.GetComponent<Text>().text = "Press 'E' to repair the mirror and escape!";
				if(Input.GetAxis ("Collect") > 0)
				{
					pickup.Play ();
					gameRunning = false;
					winText.GetComponent<Text>().enabled = true;
					winText1.GetComponent<Text>().enabled = true;
				}
			}
			else{
				MirrorText.GetComponent<Text>().text = "You have only collected " + shardsCollected + "/10 shards!";
			}
		}
	}

	void OnTriggerExit(Collider c)
	{
		if(c.CompareTag ("Collectible"))
		{
			ShardText.GetComponent<Text> ().enabled = false;
		}
		if (c.CompareTag ("LiftTrigger")) {
				FloorText.GetComponent<Text> ().enabled = false;
		}
		if (c.CompareTag ("Mirror")) {
			MirrorText.GetComponent<Text>().enabled = false;
		}
	}

	void SpawnEnemies()
	{
		for(int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i] == null)
			{
				float leftBound = transform.position.x - 10; //Position 10 units to the left of player
				float rightBound = transform.position.x + 10; //Position 1- units to the right of player

				float spawnX = 0.0f;

				if(leftBound < 0)  //Am I too close to the left edge?
					spawnX = Random.Range (rightBound, 40); //Spawn on right hand side of player (at minimum distance)
				else if (rightBound > 40) //Am I too close to the right edge?
					spawnX = Random.Range (0, leftBound); //Spawn on the left hand side of player (at minimum distance)
				else{ //Else, pick a random side and spawn there
					int side = Random.Range (0,2);
					if(side == 0)
					{
						spawnX = Random.Range (rightBound, 40);
					}
					else
					{
						spawnX = Random.Range (0, leftBound);
					}
				}

				Vector3 spawnPos = new Vector3(spawnX, ((int)(transform.position.y)) + 0.1f, 0f);
				enemies[i] = (GameObject)Instantiate (ethereal,spawnPos, Quaternion.identity);
			}
		}
	}
}
