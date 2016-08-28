using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip menuMusic;
	public AudioClip gameMusic;
	public AudioClip endMusic;

	public AudioSource source1;
	public AudioSource source2;
	public AudioSource source3;

	// Use this for initialization
	void Start ()
	{
		PlayMenuMusic ();
	}

	public void PlayMenuMusic()
	{
		StopAllCoroutines ();
		StartCoroutine (MenuCoroutine ());
		StartCoroutine (CrossFade(source3, source1));
	}

	public void PlayGameMusic()
	{
		StopAllCoroutines ();
		StartCoroutine (CrossFade(source1, source2));
	}

	public void PlayEndMusic()
	{
		StopAllCoroutines ();
		StartCoroutine (CrossFade (source2, source3));
	}

	public AudioSource GetAudioSource()
	{
		return source2;
	}

	IEnumerator MenuCoroutine()
	{
		if (!source1.isPlaying)
			source1.Play ();
		
		while (true)
		{
			yield return new WaitForSeconds (menuMusic.length - Time.deltaTime);
			source1.pitch *= -1;
		}
	}

	IEnumerator CrossFade(AudioSource sc1, AudioSource sc2)
	{
		if (!sc2.isPlaying)
			sc2.Play ();
		
		for (float i = 0; i <= 1; i += 0.1f)
		{
			sc1.volume = 1f - i;
			sc2.volume = i;
			yield return new WaitForSeconds (0.1f);
		}
		sc1.volume = 0;
		sc1.Stop ();
		sc2.volume = 1;
	}
}
