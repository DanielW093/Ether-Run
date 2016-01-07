using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	
	
	//public float x = 0.7f;
	//public float y = -0.11f;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(Example());
	}
	
	
	void Update()
	{
		//GameObject test;
		//test = new GameObject("Test");
		//test.transform.position = new Vector2(x, y);
		//print(test.transform.position.x);
	}
	
	IEnumerator Example()
	{
		yield return new WaitForSeconds(5);
        float fadeTime = GameObject.Find("FadeScreen").GetComponent<Fade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel("test");
		Application.LoadLevel("MainMenu");
	}
}