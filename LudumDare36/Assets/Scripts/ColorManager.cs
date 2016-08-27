using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour {

	public Color startColor;
	public Image[] images;
	public Text[] texts;

	private GameManager gm;

	private Color currentColor;

	private Color target;

	void Start()
	{
		currentColor = startColor;
		target = Color.blue;
		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		UpdateColor ();
		if (currentColor == Color.blue)
			target = Color.red;
		else if (currentColor == Color.red)
			target = Color.green;
		else if (currentColor == Color.green)
			target = Color.yellow;
		else if (currentColor == Color.yellow)
			target = Color.cyan;
		else if (currentColor == Color.cyan)
			target = Color.magenta;
		else if (currentColor == Color.magenta)
			target = Color.blue;
		if (gm.gameState == GameManager.GameState.PLAY)
			currentColor = Color.Lerp (currentColor, target, Time.deltaTime * 0.3f);
	}

	void UpdateColor()
	{
		for (int i = 0; i < images.Length; ++i)
		{
			images [i].color = currentColor;
		}
		for (int i = 0; i < texts.Length; ++i)
		{
			texts [i].color = currentColor;
		}
		Camera.main.backgroundColor = currentColor;
	}
}
