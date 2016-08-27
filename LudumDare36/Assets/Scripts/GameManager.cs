using UnityEngine;
using System.Collections;

// TODO : anim canvas when hitting play / going back to the menu
// TODO : draw obstacles for the gears
// TODO : the player gear can go in and out of the main gear
// TODO : ambient camera color
// TODO : particles on death
// TODO : find music theme
// TODO : add power ups
public class GameManager : MonoBehaviour {

	[Header("Game Parameters")]
	public float velocityMin;
	public float velocityMax;
	public float velocityStep;
	private float _velocity = 0;

	public enum GameState {MENU, PLAY, PAUSE};

	private GameState _gameState = GameState.MENU;

	public GameState gameState
	{
		get { return _gameState; }
	}

	public float velocity
	{
		get { return _velocity; }
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (_gameState)
		{
		case GameState.PLAY:
			_velocity = Mathf.Clamp (_velocity + velocityStep, velocityMin, velocityMax);
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
	}

	public void GoToMenu()
	{
		_gameState = GameState.MENU;
		_velocity = 0;
	}
}
