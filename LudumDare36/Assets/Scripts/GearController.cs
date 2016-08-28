using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GearController : MonoBehaviour {

	[SerializeField] private GameObject gearArtifactOut;
	[SerializeField] private GameObject gearArtifactIn;

	private GameManager gm;

	private List<GameObject> artifactsIn = new List<GameObject>();
	private List<GameObject> artifactsOut = new List<GameObject>();

	private Vector3 vecRef;

	// Use this for initialization
	void Start ()
	{
		/*float radiusOut = GetComponent<SpriteRenderer> ().bounds.size.x / 2.13f;
		float radiusIn = GetComponent<SpriteRenderer> ().bounds.size.x / 3.19f;*/

		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();

		// Outer artifacts
		/*for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI / 17.0f)
		{
			GameObject go = (GameObject)Instantiate (gearArtifactOut, new Vector3 (transform.position.x + Mathf.Cos (i) * radiusOut,
													transform.position.y + Mathf.Sin (i) * radiusOut,
													transform.position.z),
													Quaternion.Euler(0, 0, i * Mathf.Rad2Deg - 90f));
			go.transform.parent = transform;
			go.name = "Outer";
			//go.SetActive (false);
			artifactsOut.Add (go);
		}*/

		// Inner artifacts
		/*for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI / 8.5f)
		{
			GameObject go = (GameObject)Instantiate (gearArtifactIn, new Vector3 (transform.position.x + Mathf.Cos (i) * radiusIn,
																					transform.position.y + Mathf.Sin (i) * radiusIn,
																					transform.position.z),
														Quaternion.Euler(0, 0, i * Mathf.Rad2Deg - 90f));
			go.transform.parent = transform;
			go.name = "Inner";
			//go.SetActive (false);
			artifactsIn.Add (go);
		}*/

		GameObject[] gms = GameObject.FindGameObjectsWithTag ("Artifact");
		foreach (GameObject go in gms)
		{
			if (go.name == "Inner")
				artifactsIn.Add (go);
			else
				artifactsOut.Add (go);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gm.gameState == GameManager.GameState.PLAY)
		{
			UpdatePosition ();
		}
	}

	public void Reset()
	{
		foreach (GameObject go in artifactsIn)
		{
			if (go.transform.childCount > 0)
				go.transform.GetChild(0).gameObject.SetActive (false);
		}
		foreach (GameObject go in artifactsOut)
		{
			if (go.transform.childCount > 0)
				go.transform.GetChild(0).gameObject.SetActive (false);
		}
		serieToSpawn = 0;
		currentSpacesLength = 0;
		artifactMinSpawnChance = 10;
		artifactMaxSpawnChance = 60;		
	}

	void UpdatePosition()
	{
		transform.Rotate (new Vector3 (0, 0, gm.velocity * -0.5f * gm.direction));

		if (Input.GetKeyDown(KeyCode.A))
		{
			foreach (GameObject go in artifactsIn)
				go.SetActive (true);
		}
	}

	// The rules :
	// This is called from the game manager when a artifact crosses
	// Can't have more than 4 in a row
	// Each serie as at least 2 empty spaces between each
	// Chances to have artifacts increases over time
	private int serieToSpawn = 0;
	private int currentSpacesLength = 0;
	private float artifactMinSpawnChance = 10;
	private float artifactMaxSpawnChance = 60;
	private ArtifactType artifactType = ArtifactType.OUTER;
	private enum ArtifactType {INNER, OUTER, EMPTY};

	public void GenerateArtifacts(GameObject artifactCollided)
	{
		if (gm.gameState != GameManager.GameState.PLAY)
			return;
		
		// First, we chack if we do a wall
		if (serieToSpawn == 0 && Random.Range (artifactMinSpawnChance, artifactMaxSpawnChance) < 50f)
		{
			if (artifactCollided.transform.childCount > 0)
			{
				//artifactCollided.transform.GetChild (0).gameObject.SetActive (false);
				//artifactCollided.GetComponentInChildren<ArtifactController> ().Hide ();
				artifactCollided.transform.GetChild (0).gameObject.GetComponent<ArtifactController>().Hide();
			}
			
			++currentSpacesLength;
			return;
		}

		// Determines then size of the artefact if not already done
		if (serieToSpawn == 0 && currentSpacesLength >= 8)
		{
			// Determines what kind of artifact we are going to spawn
			artifactType = artifactType == ArtifactType.INNER ? ArtifactType.OUTER : ArtifactType.INNER;

			if (artifactType == ArtifactType.INNER)
				serieToSpawn = Random.Range (1, 3);
			else
				serieToSpawn = Random.Range (1, 4);

			Debug.Log ("Going to generate a " + serieToSpawn + " serie TYPE = " + artifactType);

			ManageArtifact (artifactCollided);
			currentSpacesLength = 0;

			// Each new serie we increase the chance of artifacts
			artifactMinSpawnChance = Mathf.Clamp(artifactMinSpawnChance + 0.01f, 10, 40);
			artifactMinSpawnChance = Mathf.Clamp(artifactMaxSpawnChance + 0.01f, 60, 90);
		}
		else
		{
			if (serieToSpawn > 0)
			{
				ManageArtifact (artifactCollided);
			}
			else
			{
				if (artifactCollided.transform.childCount > 0)
				{
					//artifactCollided.transform.GetChild (0).gameObject.SetActive (false);
					artifactCollided.transform.GetChild (0).gameObject.GetComponent<ArtifactController>().Hide();
					//artifactCollided.GetComponentInChildren<ArtifactController> ().Hide ();
				}
				++currentSpacesLength;
			}
		}
	}

	// Return true if the artifact spawned
	bool ManageArtifact(GameObject artifactCollided)
	{
		if (artifactCollided.transform.childCount == 0)
			return false;
		
		if (artifactCollided.name == "Inner" && artifactType == ArtifactType.INNER)
		{
			artifactCollided.transform.GetChild (0).gameObject.SetActive (true);
			--serieToSpawn;
			return true;
		}
		else if (artifactCollided.name == "Outer" && artifactType == ArtifactType.OUTER)
		{
			artifactCollided.transform.GetChild (0).gameObject.SetActive (true);
			--serieToSpawn;
			return true;
		}
		else
			//artifactCollided.transform.GetChild (0).gameObject.SetActive (false);
			artifactCollided.transform.GetChild (0).gameObject.GetComponent<ArtifactController>().Hide();
		return false;
	}
}
