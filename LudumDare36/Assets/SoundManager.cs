using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip menuMusic;
	public AudioClip gameMusic;
	public AudioClip endMusic;

	public AudioSource source1;
	public AudioSource source2;
	public AudioSource source2bis;
	public AudioSource source3;

	private AudioSource source2Original;
	private AudioSource source2BisOriginal;

	// Use this for initialization
	void Start ()
	{
		source2Original = source2;
		source2BisOriginal = source2bis;
		PlayMenuMusic ();
	}

	public void PlayMenuMusic()
	{
		StopAllCoroutines ();
		StartCoroutine (CrossFade(source3, source1));
	}

	public void PlayGameMusic()
	{
		source2 = source2Original;
		source2bis = source2BisOriginal;
		StopAllCoroutines ();
		StartCoroutine (CrossFade(source1, source2));
		StartCoroutine (GameMusicLoop ());
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

	IEnumerator GameMusicLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds (gameMusic.length * 0.95f);
			StartCoroutine (CrossFade (source2, source2bis));
			AudioSource tmp = source2;
			source2 = source2bis;
			source2bis = tmp;
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
