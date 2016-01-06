using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour {

    public Canvas CanvasObject;
    private bool fadeActive = false;

    void Start()
    {
        CanvasObject.GetComponent<Canvas>().enabled = false;
    }
    void Update()
    {
        if(fadeActive == true){
        StartCoroutine(ChangeLevel());
        }
    }

    IEnumerator ChangeLevel()
    {
        float fadeTime = GameObject.Find("FadeScreen").GetComponent<Fade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel("GameScene");
    }

	public void getButton(string button)
	{
		switch (button) 
		{
		case "PlayButton": 
			Debug.Log("playButtonPressed");
            fadeActive = true;
			break;

        case "CreditButton":
            Debug.Log("CreditButtonPressed");
            fadeActive = false;
            CanvasObject.GetComponent<Canvas>().enabled = true;
            break;


        case "BackButton":
            CanvasObject.GetComponent<Canvas>().enabled = false;
            break;
		

		}
	}
}
