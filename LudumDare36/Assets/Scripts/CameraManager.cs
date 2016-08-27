using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private GameManager gm;

	void Awake()
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	// Called at the end of the play anim
	public void BeginPlay()
	{
		gm.StartGame ();
	}


	// Called at the end of the menu anim
	public void BeginMenu()
	{
		gm.ResetGear ();
	}
}
