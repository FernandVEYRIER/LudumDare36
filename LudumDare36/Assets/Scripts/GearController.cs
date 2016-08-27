using UnityEngine;
using System.Collections;

public class GearController : MonoBehaviour {

	private GameManager gm;

	private Vector3 vecRef;

	// Use this for initialization
	void Start ()
	{
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gm.gameState == GameManager.GameState.PLAY)
			UpdatePosition ();
	}

	void UpdatePosition()
	{
		transform.Rotate (new Vector3 (0, 0, gm.velocity * -0.5f * gm.direction));
	}
}
