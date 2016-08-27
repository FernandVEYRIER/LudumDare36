using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[Header("Boundaries")]
	public Transform topPos;
	public Transform downPos;

	private GameManager gm;

	private enum Position
	{UP, DOWN};

	private Position position = Position.UP;
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
		{
			UpdateInputs ();
			UpdatePosition ();
		}
	}

	void UpdateInputs()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			position = Position.UP;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			position = Position.DOWN;
		}
	}

	void UpdatePosition()
	{
		transform.position = Vector3.SmoothDamp (transform.position, position == Position.UP ? topPos.position : downPos.position, ref vecRef, 0.05f);
		transform.Rotate (new Vector3 (0, 0, gm.velocity * (position == Position.UP ? 1 : -1)));
	}
}
