using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	
	
	//public float x = 0.7f;
	//public float y = -0.11f;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(StartSplashscreen());
	}
	
	
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			StartCoroutine(LoadMenu());
		}

		//GameObject test;
		//test = new GameObject("Test");
		//test.transform.position = new Vector2(x, y);
		//print(test.transform.position.x);
	}
	
	IEnumerator StartSplashscreen()
	{
		yield return new WaitForSeconds(5);
        //Application.LoadLevel("test");
		StartCoroutine(LoadMenu());
	}


	IEnumerator LoadMenu()
	{
		float fadeTime = GameObject.Find("FadeScreen").GetComponent<Fade>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel("MainMenu");
		StopAllCoroutines();
	}
}