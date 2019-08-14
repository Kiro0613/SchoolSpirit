using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNodeConstructor {
    public float timer;
    public AudioSource audioSource;
    public AudioClip sound;

    public SoundNodeConstructor(float timerInit, AudioClip soundInit) {
        timer = timerInit;
    }
}