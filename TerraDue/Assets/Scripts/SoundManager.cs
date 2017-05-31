using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null; 

	public AudioSource effects;
	public AudioSource music;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}

	public void playSoundEffect(AudioClip clip) {
		effects.PlayOneShot (clip, 1f);
	}

	public void setBackgroundMusic(AudioClip clip) {
		music.clip = clip;
		music.Play ();
	}
}
