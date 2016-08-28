using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// TODO : fix algo

public class GameManager : MonoBehaviour {

	[Header("Game Parameters")]
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform playerSpawnPoint;
	[SerializeField] private GearController gearController;
	public float velocityMin;
	public float velocityMax;
	public float velocityStep;
	private float _velocity = 0;
	private float _direction = 1;

	[Header("Canvas")]
	[SerializeField] private GameObject canvasGame;
	[SerializeField] private GameObject canvasGameOver;
	[SerializeField] private Text textScore;
	[SerializeField] private Text textBestScore;
	[SerializeField] private GameObject imageWarning;

	[Header("Sound")]
	[SerializeField] private SoundManager soundManager;
	[SerializeField] private AudioClip beepSound;

	public enum GameState {MENU, PLAY, PAUSE};

	private GameState _gameState = GameState.MENU;
	private float score = 0;
	private DataManager dm;
	private AudioSource audioSource;

	public GameState gameState
	{
		get { return _gameState; }
	}

	public float velocity
	{
		get { return _velocity; }
	}
	public float direction
	{
		get { return _direction; }
	}

	// Use this for initialization
	void Start ()
	{
		dm = GetComponent<DataManager> ();
		audioSource = soundManager.GetAudioSource ();
		canvasGame.SetActive (false);
		canvasGameOver.SetActive (false);
		textBestScore.text = "Best : " + dm.BestScore;
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (_gameState)
		{
		case GameState.PLAY:
			_velocity = Mathf.Clamp (_velocity + velocityStep, velocityMin, velocityMax);
			score += _velocity * Time.deltaTime;
			textScore.text = ((int)score).ToString ();
			/*Debug.Log (velocity);*/
			if (Input.GetKeyDown (KeyCode.A))
				StartCoroutine (EventCoroutine ());
			break;
		}
	}

	public void SetGameState(GameState state)
	{
		_gameState = state;
	}

	// Called from anim in camera behaviour
	public void StartGame()
	{
		_gameState = GameState.PLAY;
		_velocity = velocityMin;
		canvasGame.SetActive (true);
		canvasGameOver.SetActive (false);
		score = 0;
		Instantiate (playerPrefab, playerSpawnPoint.position, Quaternion.identity);
		ResetGear ();
		soundManager.PlayGameMusic ();
	}

	public void Quit()
	{
		#if UNITY_WEBGL
		Application.ExternalEval("window.close()");
		#else
		Application.Quit();
		#endif
	}

	public void GoToMenu()
	{
		_gameState = GameState.MENU;
		_velocity = 0;
		canvasGame.SetActive (false);
		canvasGameOver.SetActive (false);
		soundManager.PlayMenuMusic ();
	}

	public void ResetGear()
	{
		gearController.Reset ();
	}

	public void GameOver()
	{
		_gameState = GameState.PAUSE;
		canvasGameOver.SetActive (true);
		StopCoroutine ("EventCoroutine");
		dm.SaveData ((int)score);
		textBestScore.text = "Best : " + dm.BestScore;
		soundManager.PlayEndMusic ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		gearController.GenerateArtifacts (col.gameObject);
	}

	IEnumerator EventCoroutine()
	{
		// This makes the warning flash with sound
		Image sr = imageWarning.GetComponent<Image> ();
		GetComponent<ColorManager> ().enabled = false;
		imageWarning.SetActive (true);

		for (int i = 0; i < 5; ++i)
		{
			Camera.main.GetComponent<AudioSource> ().PlayOneShot (beepSound, 0.3f);
			//audioSource.PlayOneShot (beepSound, 0.3f);
			for (float c = 0; c <= 1; c += 0.05f)
			{
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, c);
				yield return new WaitForSeconds (0.01f);
			}
			for (float c = 1; c >= 0; c -= 0.05f)
			{
				sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, c);
				yield return new WaitForSeconds (0.01f);
			}
		}
		imageWarning.SetActive (false);
		GetComponent<ColorManager> ().enabled = true;

		// And this is the pitch drop
		float initialPitch = audioSource.pitch;
		float targetPitch = initialPitch * -1;
		float targetDirection = -_direction;

		// Gets current audio source from the manager
		audioSource = soundManager.GetAudioSource ();
		for (float t = 0; t < 1; t += 0.05f)
		{
			audioSource.pitch = Mathf.SmoothStep (audioSource.pitch, 0, t);
			_direction = Mathf.SmoothStep (_direction, 0, t);
			yield return new WaitForSeconds (0.05f);
		}
		for (float t = 0; t < 1; t += 0.05f)
		{
			audioSource.pitch = Mathf.SmoothStep (audioSource.pitch, targetPitch, t);
			_direction = Mathf.SmoothStep (_direction, targetDirection, t);
			yield return new WaitForSeconds (0.05f);
		}
		_direction = targetDirection;
		audioSource.pitch = targetPitch;
	}
}
