using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour {

	//Enemy Variables
	private GameObject[] enemies = new GameObject[5];
	public GameObject ethereal;

	private GameObject player;

	public float minSpawnDist = 10f;
	public float maxSpawnDist = 25f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i] == null)
			{
				float rightMin = player.transform.position.x + minSpawnDist;
				float rightMax = player.transform.position.x + maxSpawnDist;
				
				float spawnX = 0.0f;

				spawnX = Random.Range (rightMin, rightMax);
						
				Vector3 spawnPos = new Vector3(spawnX, ((int)(player.transform.position.y) + 0.035f), 0f);
				enemies[i] = (GameObject)Instantiate (ethereal,spawnPos, Quaternion.identity);
			}
		}
	}

	public void DestroyAll()
	{
		for(int i = 0; i < enemies.Length; i++)
		{
			enemies[i].GetComponent<EnemyScript>().Destroy ();
		}
	}
}
