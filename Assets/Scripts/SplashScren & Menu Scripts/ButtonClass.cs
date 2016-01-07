using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour {

    public Canvas CanvasObject;
    private bool fadeActive = false;

    //Audio sources
    public AudioSource startButtonAudio;
    public AudioSource OtherButtonAudio;

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
            startButtonAudio.Play();
			Debug.Log("playButtonPressed");
            fadeActive = true;
			break;

        case "CreditButton":
            Debug.Log("CreditButtonPressed");
            startButtonAudio.Stop();
            OtherButtonAudio.Play();
            fadeActive = false;
            CanvasObject.GetComponent<Canvas>().enabled = true;
            break;


        case "BackButton":
            OtherButtonAudio.Play();
            CanvasObject.GetComponent<Canvas>().enabled = false;
            break;
		

		}
	}
}
