using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	public GameObject room1;
	public GameObject room2;
	public GameObject room3;
	public GameObject stairs;
	public GameObject door;
	public GameObject wall;
	public GameObject shard;

	public int roomSize = 5;
	public int roomSizeH = 2;
	public double worldWidth;
	public double worldHeight;
	public int wallSpawnChance = 50;

	GameObject[] rooms;

	const int shardAmount = 10;
	private int shardsSpawned = 0;
	private int shardPass = 1;

	// Use this for initialization
	void Start () {
		rooms = new GameObject[(int)worldHeight * (int)worldWidth];

		genworld ();
	}


	void genworld()
	{
		double middle = (worldWidth/2)-0.5;

		for (int x = 0; x <worldWidth; x++)
		{
			for (int y = 0; y <worldHeight; y++)
			{
				int spawnWall = Random.Range (0,101);

				GameObject room = room1;
				if (x == 0 || x == worldWidth-1 || x != middle)
				{
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
				}

				if(x == 0)
				{
					rooms[x*y] = (GameObject)Instantiate(room,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
					GameObject wallObj = (GameObject)Instantiate(wall,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
					wallObj.transform.parent = this.transform;
				}
				else if(x == worldWidth-1)
				{
					rooms[x*y] = (GameObject)Instantiate(room,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
					GameObject wallObj = (GameObject)Instantiate(wall,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
					wallObj.transform.Rotate(new Vector3(0,1,0),180f);
					wallObj.transform.parent = this.transform;
				}
				else if(x != middle)
				{
					rooms[x*y] = (GameObject)Instantiate(room,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);

					if(x < middle && spawnWall <= wallSpawnChance)
					{
						//Wall Spawn
						GameObject doorObj = (GameObject)Instantiate (door, new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
						doorObj.transform.parent = this.transform;
					}
					else if (x > middle && spawnWall <= wallSpawnChance)
					{
						//Wall Spawn with rotation
						GameObject doorObj = (GameObject)Instantiate (door, new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
						doorObj.transform.Rotate(new Vector3(0,1,0),180f);
						doorObj.transform.parent = this.transform;
					}
				}
				else
				{
					rooms[x*y] = (GameObject)Instantiate(stairs,new Vector3(roomSize*x,roomSizeH*y,0),Quaternion.identity);
					if(y == 0)
						rooms[x*y].GetComponent<RoomData>().canGoDown = false;
					else if (y == worldHeight-1)
						rooms[x*y].GetComponent<RoomData>().canGoUp = false;
				}

				rooms[x*y].gameObject.transform.parent = this.transform;
			}
		}
		
		while(shardsSpawned < 10)
		{
			for (int x = 0; x <worldWidth; x++) {
				for (int y = 0; y < worldHeight; y++) {
					if(x != middle && shardsSpawned < 10 && y != 9)
					{
						int randomThresh = (5 * (10-y))*shardPass;
						int random = Random.Range(0,101);
						if(random <= randomThresh && rooms[x*y].GetComponent<RoomData>().hasShard == false)
						{
							GameObject roomShard = (GameObject)Instantiate (shard, new Vector3(roomSize*x,roomSizeH*y-0.1f,0),Quaternion.identity);
							roomShard.transform.Rotate(0f,0f,90f);
							shardsSpawned++;
							rooms[x*y].GetComponent<RoomData>().hasShard = true;
						}
					}
				}
			}
			shardPass++;
		}


	}

	int[] splitInt(int n)
	{
		List<int> digits = new List<int> ();
		while (n > 0) 
		{
			digits.Add(n%10);
			n/=10;
		}
		digits.Reverse ();
		return digits.ToArray();
	}

	string printArray(int[] i)
	{
		string s = "";
		foreach(int p in i)
		{
			s += p;
		}
		return s;
	}
}
