using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GameManager{

	public static bool PlayAudio = true;
	public static bool DisplayTutorial = true;

	public static void UpdateSettings()
	{
		BinaryFormatter bf = new BinaryFormatter();

		bool[] settings = new bool[2];

		settings[0] = PlayAudio;
		settings[1] = DisplayTutorial;

		FileStream settingsFile = File.Open(Application.persistentDataPath + "/settings.gd", FileMode.OpenOrCreate);
		bf.Serialize(settingsFile, settings);
		settingsFile.Close();
	}

	public static void LoadSettings()
	{
		BinaryFormatter bf = new BinaryFormatter();
		bool[] settings;
		FileStream settingsFile = File.Open(Application.persistentDataPath + "/settings.gd", FileMode.OpenOrCreate);
		if(settingsFile.Length != 0)
		{
			settings = (bool[])bf.Deserialize(settingsFile);
			PlayAudio = settings[0];
			DisplayTutorial = settings[1];
		}
		settingsFile.Close();
	}
}
