using UnityEngine;
using System.Collections;

public class ArtifactController : MonoBehaviour {

	private Animator animator;
	
	public void Hide()
	{
		if (!gameObject.activeSelf)
			return;
		
		if (animator == null)
			animator = GetComponent<Animator> ();

		animator.SetTrigger ("Hide");
	}

	public void OnHideAnimEnd()
	{
		gameObject.SetActive (false);
	}
}
