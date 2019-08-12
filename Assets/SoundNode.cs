using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void playSound(AudioClip sound) {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.Play();
    }
}
