using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	private GameObject player;

	public GameObject room1;
	public GameObject room2;
	public GameObject room3;
	public GameObject door;

	public float roomSize = 7.49f;
	public int maxRooms = 5;
	public int wallSpawnChance = 50;

	private int middleRoom;

	List<GameObject> rooms = new List<GameObject> ();

	Vector3 roomPosition = new Vector3(0f, 0f, 0f);

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		middleRoom = (int)(maxRooms / 2) + (int)(maxRooms % 2);
		genworld ();
	}

	void Update()
	{
		if(PlayerScript.gameRunning)
		{
			if (player.transform.position.x >= (roomPosition.x - (middleRoom * roomSize))) {
				UpdateWorld();
			}
		}
	}


	void genworld()
	{
		for(int x = 0; x < maxRooms; x++)
		{
			int spawnWall = Random.Range (0,101); //Generate random number
			
			GameObject room = room1; //Set default room

			int roomMesh = Random.Range (0,3);
			switch(roomMesh){
			case 0: 
				room = room1;
				break;
			case 1:
				room = room2;
				break;
			case 2:
				room = room3;
				break;
			}


			//Spawn room
			GameObject roomObj = (GameObject)Instantiate(room,roomPosition,Quaternion.identity);
			rooms.Add(roomObj);
				
			if(spawnWall <= wallSpawnChance) //If random chance to spawn wall succeeds
			{
				//Door Spawn with rotation
				GameObject doorObj = (GameObject)Instantiate (door, roomPosition,Quaternion.identity);
				doorObj.transform.Rotate(new Vector3(0,1,0),180f);
				doorObj.transform.parent =  roomObj.transform;
			}

			roomPosition.x += roomSize;
		}
	}

	void UpdateWorld()
	{
		Destroy (rooms [0]);
		rooms.RemoveAt (0);

		int spawnWall = Random.Range (0,101); //Generate random number
		
		GameObject room = room1; //Set default room
		
		int roomMesh = Random.Range (0,3);
		switch(roomMesh){
		case 0: 
			room = room1;
			break;
		case 1:
			room = room2;
			break;
		case 2:
			room = room3;
			break;
		}

		//Spawn room
		GameObject roomObj = (GameObject)Instantiate(room,roomPosition,Quaternion.identity);
		rooms.Add(roomObj);
			
		if(spawnWall <= wallSpawnChance) //If random chance to spawn wall succeeds
		{
			//Door Spawn with rotation
			GameObject doorObj = (GameObject)Instantiate (door, roomPosition,Quaternion.identity);
			doorObj.transform.Rotate(new Vector3(0,1,0),180f);
			doorObj.transform.parent = roomObj.transform;
		}
		
		roomPosition.x += roomSize;
	}
}
