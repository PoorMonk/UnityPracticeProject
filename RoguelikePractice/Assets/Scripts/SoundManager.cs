using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource BgMusic;
    public AudioSource efxMusic;
    static public SoundManager instance = null;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
	}
	
	public void PlaySingle(AudioClip clip)
    {
        efxMusic.clip = clip;
        efxMusic.Play();
    }

    public void RandomMusic(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxMusic.clip = clips[randomIndex];
        efxMusic.pitch = randomPitch;
        efxMusic.Play();
    }
}
