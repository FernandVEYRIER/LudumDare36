using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {

	private const string savePath = "/save.dat";

	public int BestScore 
	{
		get { return bestScore; }
	}
	private int bestScore = 0;

	void Awake()
	{
		LoadData ();
	}

	void LoadData()
	{
		if (File.Exists(Application.persistentDataPath + savePath))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + savePath, FileMode.Open);
			if (file.Length <= 0)
			{
				file.Close ();
				SaveData ();
			}
			else
				bestScore = (int)bf.Deserialize (file);
			file.Close ();
		}
		else
		{
			File.Create (Application.persistentDataPath + savePath);
			SaveData ();
		}
	}

	public void SaveData(int newScore = 0)
	{
		if (newScore > bestScore)
			bestScore = newScore;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + savePath);
		bf.Serialize (file, bestScore);
		file.Close ();
	}
}
