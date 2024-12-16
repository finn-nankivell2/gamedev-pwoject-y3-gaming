using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume = 0.5f;

    [Serializable]
    public struct AudioRef {
        public string key;
        public AudioClip soundEffect;

        public AudioRef(string k, AudioClip a) {
            key = k;
            soundEffect = a;
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
        foreach(var aRef in soundEffects) {
            if (aRef.key == key) {
                return aRef.soundEffect;
            }
        }
        return null;
    }

    public void Play(string key, Vector3 position) {
        AudioClip sound = GetSound(key);
        AudioSource.PlayClipAtPoint(sound, position, volume);
    }
}
