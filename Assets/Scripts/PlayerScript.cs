using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public static bool gameRunning = true;

	private float speed;
	public float runSpeed;
	public float walkSpeed;
	public float batteryDrainRate;
	public float batteryRechargeRate;
	CharacterController controller;

	//Torch turning variables
	private Vector3 tTarget;
	const float rotateSpeed = 10;
	public GameObject torch;
	//Player data
	public int maxHealth;
	private int health;
	private double battery = 100;
	//Torch Focusing Variables
	public bool isFocusing;
	private Light spotlight;
	const float dimInt = 0.6f;
	const float focInt = 1.2f;
	//Torch Materials
	public Material dimMat;
	public Material brightMat;
	//Audio sources
	public AudioSource torchClick;
	public AudioSource torchHum;
	public AudioSource pickup;

	//Animation
	public Animator playerAnim;

	//UI Objects
	public Image[] healthbars = new Image[5];
	public Image[] batterybars = new Image[10];
	public GameObject scoreText;

	// Use this for initialization
	void Start () {
		health = maxHealth;

		speed = runSpeed;

		controller = this.GetComponent<CharacterController> (); //Get character controller
		isFocusing = false; //Set is focusing to false

		spotlight = gameObject.GetComponentInChildren<Light> (); //Get the spotlight
	}
	
	// Update is called once per frame
	void Update () {
		int score = (int)(transform.position.x*2);
		scoreText.GetComponent<Text> ().text = "Score: " + score; //Update score text

		//Update healthbar
		for(int i = 0; i < healthbars.Length; i++)
		{
			if(i+1 > health)
				healthbars[i].enabled = false;
			else
				healthbars[i].enabled = true;
		}

		//Update battery bar
		int totalBars = (int)battery/10; //How many bars should be visible?

		for(int i = 0; i < batterybars.Length; i++) //Make necessary bars not visible
		{
			if(i > totalBars)
				batterybars[i].enabled = false;
			else
				batterybars[i].enabled = true;
		}
		if(totalBars < 10) //Modify the alpha of the active bar
		{
			Color newCol = batterybars[totalBars].color;
			newCol.a = (float)((battery - (totalBars*10))/10);
			batterybars[totalBars].color = newCol;
		}

		if (gameRunning) {
			
			if (health == 0) {
				gameRunning = false;


				//TODO: LOSING
			}
			//Calculate Velocity
			Vector3 velocity = transform.right * speed;
			//Move Player
			controller.Move (velocity * Time.deltaTime);
					
			//Rotate torch towards mouse or touch
			Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
			tTarget = Camera.main.ScreenToWorldPoint (mousePos);
			tTarget.z = torch.transform.position.z;
			Vector3 targetDir = tTarget - torch.transform.position;
			float step = rotateSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (torch.transform.forward, targetDir, step, 0.0F);

			torch.transform.rotation = Quaternion.LookRotation (newDir);



			//Mobile input
			if(Application.isMobilePlatform)
			{

			}
			else{
				if (Input.GetMouseButtonDown (0) && battery > 0) {
					torchClick.Play ();
					torchHum.Play ();
					speed = walkSpeed;
					playerAnim.speed = 0.75f;
					isFocusing = true;
				}

				if (Input.GetMouseButtonUp (0) || battery <= 0) {
					torchClick.Play ();
					torchHum.Stop ();
					speed = runSpeed;
					playerAnim.speed = 1.0f;
					isFocusing = false;
				}

				if (isFocusing && battery > 0) {
					spotlight.intensity = focInt;
					GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = brightMat;
					battery -= batteryDrainRate * Time.deltaTime;
				}

				if (!isFocusing && battery < 100) {
					//Battery recharges when not being focused, and player is moving
					spotlight.intensity = dimInt;
					GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = dimMat;
					battery += batteryRechargeRate * Time.deltaTime;
				}
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
}
