using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScoreData{

	int score;
	string name;

	public ScoreData(string n, int s)
	{
		name = n;
		score = s;
	}

	public int GetScore()
	{
		return score;
	}

	public string GetName()
	{
		return name;
	}

	public void SetScore(int s)
	{
		score = s;
	}

	public void SetName(string n)
	{
		name = n;
	}
}
