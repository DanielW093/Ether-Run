using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class HighscoreScript {

	public static ScoreData[] highscores = new ScoreData[10];

	public static void UpdateHighscores()
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream highscoreFile = File.Open(Application.persistentDataPath + "/highscores.gd", FileMode.OpenOrCreate);
		bf.Serialize(highscoreFile, highscores);
		highscoreFile.Close();
	}

	public static void LoadHighscores()
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream highscoreFile = File.Open(Application.persistentDataPath + "/highscores.gd", FileMode.OpenOrCreate);

		if(highscoreFile.Length != 0)
		{
			highscores = (ScoreData[])bf.Deserialize(highscoreFile);
		}
		highscoreFile.Close();
	}
}
