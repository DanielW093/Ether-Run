using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour {

	public Canvas MainMenuCanvas;
	public Canvas SettingsCanvas;
    public Canvas CreditsCanvas;
    private bool fadeActive = false;

    //Audio sources
    public AudioSource startButtonAudio;
    public AudioSource OtherButtonAudio;

    void Start()
    {
		MainMenuCanvas.GetComponent<Canvas>().enabled = true;
		SettingsCanvas.GetComponent<Canvas>().enabled = false;
        CreditsCanvas.GetComponent<Canvas>().enabled = false;
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
            OtherButtonAudio.Play();
			MainMenuCanvas.GetComponent<Canvas>().enabled = false;
            CreditsCanvas.GetComponent<Canvas>().enabled = true;
            break;
		case "SettingsButton":
			Debug.Log("Settings Button Pressed");
			OtherButtonAudio.Play();
			MainMenuCanvas.GetComponent<Canvas>().enabled = false;
			SettingsCanvas.GetComponent<Canvas>().enabled = true;
			break;
        case "BackButton":
            OtherButtonAudio.Play();
			MainMenuCanvas.GetComponent<Canvas>().enabled = true;
			SettingsCanvas.GetComponent<Canvas>().enabled = false;
            CreditsCanvas.GetComponent<Canvas>().enabled = false;
            break;
		

		}
	}
}
