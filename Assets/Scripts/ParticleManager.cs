using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Serializable]
    public struct ParticleRef {
        public string key;
        public GameObject particleEffect;

        public ParticleRef(string k, GameObject p) {
            key = k;
            particleEffect = p;
        }
    }

    public ParticleRef[] particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject? GetParticle(string key) {
        foreach(var pRef in particles) {
            if (pRef.key == key) {
                return pRef.particleEffect;
            }
        }
        return null;
    }

    public void Play(string key, Vector3 position) {
        GameObject particle = GetParticle(key);
        Instantiate(particle, position, Quaternion.identity);
    }

    public void Play(string key, Vector3 position, Quaternion rotation) {
        GameObject particle = GetParticle(key);
        Instantiate(particle, position, rotation);
    }

}
