using UnityEngine;
using System.Collections;

public class CameraManager : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ().StartGame ();
	}
}
