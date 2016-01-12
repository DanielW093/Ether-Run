using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour {

	public GameObject eventSystem;
	private MainMenu menuScript;

    private bool fadeActive = false;

	public InputField nameInput;

    //Audio sources
    public AudioSource startButtonAudio;
    public AudioSource OtherButtonAudio;

    void Start()
    {
		menuScript = eventSystem.GetComponent<MainMenu>();
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
		SceneManager.LoadScene("GameScene");
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
			menuScript.MainMenuCanvas.GetComponent<Canvas>().enabled = false;
			menuScript.CreditsCanvas.GetComponent<Canvas>().enabled = true;
            break;
		case "SettingsButton":
			Debug.Log("Settings Button Pressed");
			OtherButtonAudio.Play();
			menuScript.MainMenuCanvas.GetComponent<Canvas>().enabled = false;
			menuScript.SettingsCanvas.GetComponent<Canvas>().enabled = true;
			break;
		case "HighscoreButton":
			Debug.Log("Highscore Button Pressed");
			OtherButtonAudio.Play();
			menuScript.MainMenuCanvas.GetComponent<Canvas>().enabled = false;
			menuScript.HighscoreCanvas.GetComponent<Canvas>().enabled = true;
			break;
        case "BackButton":
            OtherButtonAudio.Play();
			menuScript.MainMenuCanvas.GetComponent<Canvas>().enabled = true;
			menuScript.SettingsCanvas.GetComponent<Canvas>().enabled = false;
			menuScript.CreditsCanvas.GetComponent<Canvas>().enabled = false;
			menuScript.HighscoreCanvas.GetComponent<Canvas>().enabled = false;
            break;
		case "ScoreSubmitButton":
			OtherButtonAudio.Play();
			SubmitHighscore();
			menuScript.UpdateHighscoreTable();
			menuScript.NewHighscoreCanvas.enabled = false;
			menuScript.HighscoreCanvas.enabled = true;
			MainMenu.tempScore.SetName("Player");
			MainMenu.tempScore.SetScore(0);
			break;
		}
	}

	void SubmitHighscore()
	{
		if(nameInput.text.Length > 0)
			MainMenu.tempScore.SetName(nameInput.text);

		for(int i = 0; i < HighscoreScript.highscores.Length; i++)
		{
			if(HighscoreScript.highscores[i] != null)
			{
				if(MainMenu.tempScore.GetScore() > HighscoreScript.highscores[i].GetScore())
				{
					List<ScoreData> temps = HighscoreScript.highscores.ToList();
					temps.Insert(i, MainMenu.tempScore);

					if(temps.Count > 10)
						temps.RemoveRange(10, temps.Count - 10);

					HighscoreScript.highscores = temps.ToArray();
					break;
				}
			}
			else
			{
				List<ScoreData> temps = HighscoreScript.highscores.ToList();
				temps.Insert(i, MainMenu.tempScore);

				if(temps.Count > 10)
					temps.RemoveRange(10, temps.Count - 10);

				HighscoreScript.highscores = temps.ToArray();
				break;
			}
		}

		HighscoreScript.UpdateHighscores();
	}
}
