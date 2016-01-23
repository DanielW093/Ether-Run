using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	public static ScoreData tempScore = new ScoreData("Player", 0);

	public Canvas MainMenuCanvas;
	public Canvas SettingsCanvas;
	public Canvas CreditsCanvas;
	public Canvas HighscoreCanvas;
	public Canvas NewHighscoreCanvas;

	public Text scoreText;
	public Text versionText;

	public Toggle AudioToggle;
	public Toggle TutorialToggle;

	public GameObject[] highscores = new GameObject[10];

	// Use this for initialization
	void Start () {
		MainMenuCanvas.enabled = true;
		SettingsCanvas.enabled = false;
		CreditsCanvas.enabled = false;
		HighscoreCanvas.enabled = false;
		NewHighscoreCanvas.enabled = false;

		GameManager.LoadSettings();
		HighscoreScript.LoadHighscores();
		UpdateHighscoreTable();

		if(tempScore.GetScore() > (int)0)
		{
			scoreText.text = "Final Score: " + tempScore.GetScore();
			foreach(ScoreData s in HighscoreScript.highscores)
			{
				if(s != null)
				{
					if(tempScore.GetScore() > s.GetScore())
					{
						MainMenuCanvas.enabled = false;
						NewHighscoreCanvas.enabled = true;
						break;
					}
				}
				else
				{
					MainMenuCanvas.enabled = false;
					NewHighscoreCanvas.enabled = true;
					break;
				}
			}
		}

		AudioToggle.isOn = GameManager.PlayAudio;
		TutorialToggle.isOn = GameManager.DisplayTutorial;
		versionText.text = "Version " + Application.version;
	}
	
	// Update is called once per frame
	void Update () {
		if(AudioToggle.isOn != GameManager.PlayAudio)
		{
			GameManager.PlayAudio = AudioToggle.isOn;
			Debug.Log(GameManager.PlayAudio);
			GameManager.UpdateSettings();
		}

		if(TutorialToggle.isOn != GameManager.DisplayTutorial)
		{
			GameManager.DisplayTutorial = TutorialToggle.isOn;
			Debug.Log(GameManager.DisplayTutorial);
			GameManager.UpdateSettings();
		}

		if(AudioToggle.isOn)
			AudioListener.volume = 1.0f;
		else
			AudioListener.volume = 0.0f;
	}

	public void UpdateHighscoreTable()
	{
		for (int i = 0; i < highscores.Length; i++)
		{
			if(HighscoreScript.highscores[i] != null)
			{
				highscores[i].transform.Find("NameText").GetComponent<Text>().text = HighscoreScript.highscores[i].GetName();
				highscores[i].transform.Find("ScoreText").GetComponent<Text>().text = HighscoreScript.highscores[i].GetScore().ToString();
			}
			else
			{
				highscores[i].transform.Find("NameText").GetComponent<Text>().text = " ";
				highscores[i].transform.Find("ScoreText").GetComponent<Text>().text = " ";
			}
		}
	}
}
