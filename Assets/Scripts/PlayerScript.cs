using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public static bool gameRunning = true;

	private float speed;
	public float runSpeed;
	public float walkSpeed;
	public float batteryDrainRate;
	public float batteryRechargeRate;
	public float buttonRechargeAmount;
	CharacterController controller;

	//Torch turning variables
	private Vector3 torchAimTouch;
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
	//UI
	public Canvas pauseCanvas;
	public Canvas quitCanvas;
	public Canvas tutorialCanvas;
	public Toggle dontShowAgain;

	//Animation
	public Animator playerAnim;

	//UI Objects
	public Image[] healthbars = new Image[5];
	public Image[] batterybars = new Image[10];
	public GameObject scoreText;
	public Toggle flashlightToggle;
	public Button rechargeButton;

	// Use this for initialization
	void Start () {
		pauseCanvas.enabled = false;
		quitCanvas.enabled = false;

		if(GameManager.DisplayTutorial)
		{
			gameRunning = false;
			tutorialCanvas.enabled = true;
		}
		else
		{
			gameRunning = true;
			tutorialCanvas.enabled = false;
		}

		health = maxHealth;

		speed = runSpeed;

		controller = this.GetComponent<CharacterController> (); //Get character controller
		isFocusing = false; //Set is focusing to false

		spotlight = gameObject.GetComponentInChildren<Light> (); //Get the spotlight

		torchAimTouch = new Vector3(Screen.width/2, Screen.height/2, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {

		//TODO:
		if(!gameRunning && playerAnim.enabled)
		{
			flashlightToggle.interactable = false;
			rechargeButton.interactable = false;

			playerAnim.enabled = false;
		}

		if(gameRunning && !playerAnim.enabled)
		{
			flashlightToggle.interactable = true;
			rechargeButton.interactable = true;

			playerAnim.enabled = true;
		}

		//Update score
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
			{
				Color newCol = batterybars[i].color;
				newCol.a = 1.0f;
				batterybars[i].color = newCol;
				batterybars[i].enabled = true;
			}
		}
		if(totalBars < 10) //Modify the alpha of the active bar
		{
			Color newCol = batterybars[totalBars].color;
			newCol.a = (float)((battery - (totalBars*10))/10);
			batterybars[totalBars].color = newCol;
		}

		//Is the player dead?
		if (health == 0) {
			gameRunning = false;

			//TODO: LOSING
			GameObject.Find("EnemyManager").GetComponent<EnemySpawning>().DestroyAll();
			Camera.main.transform.SetParent(null);
			Destroy(gameObject);
		}

		if (gameRunning) {

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				PauseGame();
			}

			//Make sure battery is within acceptable parameters
			if(battery > 100)
				battery = 100;
			if(battery < 0)
				battery = 0;

			//Calculate Velocity
			Vector3 velocity = transform.right * speed;
			//Move Player
			controller.Move (velocity * Time.deltaTime);

			//Mobile input
			if(Application.isMobilePlatform)
			{
				Touch[] touches = Input.touches;

				if(touches.Length > 0)
				{
					foreach (Touch t in touches)
					{
						Vector3 touchPos = new Vector3(t.position.x, t.position.y, 10.0f);

						tTarget = Camera.main.ScreenToWorldPoint (touchPos);

						if(tTarget.x > transform.position.x)
						{
							torchAimTouch = touchPos;
							tTarget.z = transform.position.z;
							Vector3 targetDir = tTarget - torch.transform.position;
							float step = rotateSpeed * Time.deltaTime;
							Vector3 newDir = Vector3.RotateTowards (torch.transform.forward, targetDir, step, 0.0F);

							torch.transform.rotation = Quaternion.LookRotation (newDir);

							break;
						}
					}
				}
				else
				{
					tTarget = Camera.main.ScreenToWorldPoint (torchAimTouch);

					if(tTarget.x > transform.position.x)
					{
						tTarget.z = transform.position.z;
						Vector3 targetDir = tTarget - torch.transform.position;
						float step = rotateSpeed * Time.deltaTime;
						Vector3 newDir = Vector3.RotateTowards (torch.transform.forward, targetDir, step, 0.0F);

						torch.transform.rotation = Quaternion.LookRotation (newDir);
					}
				}

				//Controlling torch on mobile
				if(flashlightToggle.isOn && !isFocusing && battery > 0)
				{
					ToggleTorch(true);
				}
				if(battery <= 0 && isFocusing)
				{
					flashlightToggle.isOn = false;
				}
				if(!flashlightToggle.isOn && isFocusing)
				{
					ToggleTorch(false);
				}
			}
			else{
				//Rotate torch towards mouse or touch
				Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
				if(mousePos.x > Screen.width/3)
				{
					tTarget = Camera.main.ScreenToWorldPoint (mousePos);
					tTarget.z = transform.position.z;
					Vector3 targetDir = tTarget - torch.transform.position;
					float step = rotateSpeed * Time.deltaTime;
					Vector3 newDir = Vector3.RotateTowards (torch.transform.forward, targetDir, step, 0.0F);

					torch.transform.rotation = Quaternion.LookRotation (newDir);
				}

				if (Input.GetMouseButtonDown (0) && battery > 0) {
					ToggleTorch(true);
				}

				if (Input.GetMouseButtonUp (0) || battery <= 0) {
					ToggleTorch(false);
				}
			}

			if (isFocusing && battery > 0) {
				spotlight.intensity = focInt;
				GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = brightMat;
				battery -= batteryDrainRate * Time.deltaTime;
			}

			if (!isFocusing && battery < 100) {
				//Battery recharges when not being focused, and player is moving
				rechargeButton.interactable = true;
				spotlight.intensity = dimInt;
				GameObject.FindGameObjectWithTag ("Beam").GetComponent<MeshRenderer> ().material = dimMat;
				battery += batteryRechargeRate * Time.deltaTime;
			}
			else
			{
				rechargeButton.interactable = false;
			}
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag ("Enemy"))
		{
			if(Application.isMobilePlatform)
				Handheld.Vibrate();
			c.gameObject.GetComponent<EnemyScript>().Destroy ();
			health--;
			Debug.Log (health);
		}
	}

	void ToggleTorch(bool on)
	{
		torchClick.Play();

		if(on)
		{
			torchHum.Play();
			speed = walkSpeed;
			playerAnim.speed = 0.75f;
			isFocusing = true;
		}
		else
		{
			torchHum.Stop ();
			speed = runSpeed;
			playerAnim.speed = 1.0f;
			isFocusing = false;
		}
	}

	public void RechargeButton()
	{
		if(gameRunning)
			battery += buttonRechargeAmount;
	}

	public void DismissTutorialButton()
	{
		if(dontShowAgain.isOn)
			GameManager.DisplayTutorial = false;
		else
			GameManager.DisplayTutorial = true;

		GameManager.UpdateSettings();

		tutorialCanvas.enabled = false;
		gameRunning = true;
	}

	public void PauseGame()
	{
		gameRunning = false;
		pauseCanvas.enabled = true;
	}

	public void ResumeGame()
	{
		pauseCanvas.enabled = false;
		gameRunning = true;
	}

	public void QuitButton()
	{
		pauseCanvas.enabled = false;
		quitCanvas.enabled = true;
	}

	public void CancelQuit()
	{
		quitCanvas.enabled = false;
		pauseCanvas.enabled = true;
	}

	public void QuitGame()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
