using UnityEngine;
using System.Collections;

// TODO : find music theme + inverse pitch with velocity
// TODO : add power ups
using UnityEngine.UI;


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

	public enum GameState {MENU, PLAY, PAUSE};

	private GameState _gameState = GameState.MENU;
	private float score = 0;
	private DataManager dm;

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
	}

	public void GoToMenu()
	{
		_gameState = GameState.MENU;
		_velocity = 0;
		canvasGame.SetActive (false);
		canvasGameOver.SetActive (false);
	}

	public void ResetGear()
	{
		gearController.Reset ();
	}

	public void GameOver()
	{
		_gameState = GameState.PAUSE;
		canvasGameOver.SetActive (true);
		dm.SaveData ((int)score);
		textBestScore.text = "Best : " + dm.BestScore;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		gearController.GenerateArtifacts (col.gameObject);
	}
}
