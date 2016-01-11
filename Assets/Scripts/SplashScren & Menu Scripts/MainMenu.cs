using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Toggle AudioToggle;
	public Toggle TutorialToggle;

	// Use this for initialization
	void Start () {
		GameManager.LoadSettings();

		AudioToggle.isOn = GameManager.PlayAudio;
		TutorialToggle.isOn = GameManager.DisplayTutorial;
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
}
