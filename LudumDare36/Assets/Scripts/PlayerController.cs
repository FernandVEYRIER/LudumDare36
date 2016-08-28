using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[Header("Boundaries")]
	public Transform topPos;
	public Transform downPos;
	public GameObject deathEffect;

	private GameManager gm;

	private enum Position
	{UP, DOWN};

	private Position position = Position.UP;
	private Vector3 vecRef;

	// Use this for initialization
	void Start ()
	{
		GetComponent<CircleCollider2D> ().enabled = false;
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		topPos = GameObject.Find ("TopPos").transform;
		downPos = GameObject.Find ("DownPos").transform;
		StartCoroutine (Spawn ());
	}

	IEnumerator Spawn()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		for (float i = 0; i <= 1; i += 0.1f)
		{
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, i);
			yield return new WaitForSeconds (0.1f);
		}
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1);
		GetComponent<CircleCollider2D> ().enabled = true;
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
		#if UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
				position = (position == Position.UP) ? Position.DOWN : Position.UP;
		}
		#else
		if (Input.GetKey(KeyCode.UpArrow))
		{
			position = Position.UP;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			position = Position.DOWN;
		}
		#endif
	}

	void UpdatePosition()
	{
		transform.position = Vector3.SmoothDamp (transform.position, position == Position.UP ? topPos.position : downPos.position, ref vecRef, 0.05f);
		transform.Rotate (new Vector3 (0, 0, gm.velocity * gm.direction * (position == Position.UP ? 1.5f : -1.5f)));
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Obstacle")
		{
			Die ();
		}
	}

	void Die()
	{
		Instantiate (deathEffect, transform.position, Quaternion.Euler(0, 180, 0));
		gm.GameOver ();
		Destroy (gameObject);
	}
}
