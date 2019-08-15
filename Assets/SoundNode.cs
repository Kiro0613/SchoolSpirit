using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNode : MonoBehaviour {
    public float timer;
    public float radius;
    public AudioSource audioSource;
    public AudioClip sound;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        playSound(sound);
        getEnemiesInRange();
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        //Debug.Log(timer);
        if(timer <= 0f) {
            Destroy(gameObject);
        }
    }

    private void playSound(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void getEnemiesInRange() {
        //LayerMask enemyMask = 1 << 9;
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius, 1 << 9);
        foreach(Collider enemy in objectsInRange) {
            enemy.gameObject.GetComponent<Enemy>().hearSound((radius - Vector3.Distance(transform.position, enemy.transform.position)), transform.position);
        }
        //
        //Debug.Log(objectsInRange.Length);
    }
}