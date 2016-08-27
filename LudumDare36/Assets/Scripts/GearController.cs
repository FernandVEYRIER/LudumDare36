using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GearController : MonoBehaviour {

	[SerializeField] private GameObject gearArtifactOut;

	private GameManager gm;

	private List<GameObject> artifacts = new List<GameObject>();

	private Vector3 vecRef;

	// Use this for initialization
	void Start ()
	{
		float radiusOut = GetComponent<SpriteRenderer> ().bounds.size.x / 2.09f;
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();

		for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI / 4.0f)
		{
			GameObject go = (GameObject)Instantiate (gearArtifactOut, new Vector3 (transform.position.x + Mathf.Cos (i) * radiusOut,
													transform.position.y + Mathf.Sin (i) * radiusOut,
													transform.position.z),
													Quaternion.Euler(0, 0, i * Mathf.Rad2Deg - 90f));
			go.transform.parent = transform;
			go.SetActive (false);
			artifacts.Add (go);
		}
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
