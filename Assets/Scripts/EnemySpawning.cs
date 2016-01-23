using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour {

	//Enemy Variables
	private GameObject[] enemies;
	public GameObject ethereal;
    public GameObject etherealCeil;

	public int maxEnemies;

	private GameObject player;

	public float minSpawnDist = 10f;
	public float maxSpawnDist = 25f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		enemies = new GameObject[maxEnemies];
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerScript.gameRunning)
		{
			for(int i = 0; i < enemies.Length; i++)
			{
				if(enemies[i] == null)
				{
					float rightMin = player.transform.position.x + minSpawnDist;
					float rightMax = player.transform.position.x + maxSpawnDist;
					
					float spawnX = 0.0f;

					spawnX = Random.Range (rightMin, rightMax);
							
					Vector3 spawnPos = new Vector3(spawnX, ((int)(player.transform.position.y) + 0.035f), 0f);

					if(PlayerScript.ceilingEnemiesEnabled)
					{
		                int rand = Random.Range(0, 11);

						if(rand >= 0 && rand < 6)
						{
							enemies[i] = (GameObject)Instantiate (ethereal,spawnPos, Quaternion.identity);
						}
						else
						{
							spawnPos.y = 1.55f;
							enemies[i] = (GameObject)Instantiate(etherealCeil, spawnPos, Quaternion.identity);
						}
					}
					else
					{
						enemies[i] = (GameObject)Instantiate (ethereal,spawnPos, Quaternion.identity);
					}
				}
			}
		}
	}

	public void DestroyAll()
	{
		for(int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i] != null)
				enemies[i].GetComponent<EnemyScript>().Kill ();
		}
	}
}
