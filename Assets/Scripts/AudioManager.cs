using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume = 0.5f;
    public AudioSource playerAudioSource;

    [Serializable]
    public class AudioRef {
        public string key;
        public AudioClip soundEffect;

		[Range(0f, 1.5f)]
		public float volume = 1f;

        public AudioRef(string k, AudioClip a, float v) {
            key = k;
            soundEffect = a;
            volume = v;
        }
    }

    public AudioRef[] soundEffects;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public AudioClip? GetSound(string key) {
		var aRef = GetAudioRef(key);
		if (aRef != null) {
			return aRef.soundEffect;
		}
		return null;
    }

    public AudioRef? GetAudioRef(string key) {
        foreach(var aRef in soundEffects) {
            if (aRef.key == key) {
                return aRef;
            }
        }
        return null;
    }

    public void PlaySpatial(string key, Vector3 position) {
    	AudioRef aRef = GetAudioRef(key);
        AudioClip sound = aRef.soundEffect;
        AudioSource.PlayClipAtPoint(sound, position, aRef.volume * volume);
    }

    public void Play(string key) {
    	AudioRef aRef = GetAudioRef(key);
        AudioClip sound = aRef.soundEffect;
        playerAudioSource.PlayOneShot(sound, aRef.volume * volume);
    }
}
