using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates {
    Patrolling,
    Suspicious,
    Searching,
    Chasing
}

public class Enemy : MonoBehaviour {
    public EnemyStates enemyState;
    private EnemyStates lastState;  //State enemy had on previous call
    AIConeDetection visCone;
    public GameObject indicator;   //Changes color to tell how enemy feels

    public float searchTimeout; //Enemy stops searching for player when runs out
    public float moveSpeed;
    public float lookSpeed;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        visCone = GetComponent<AIConeDetection>();
        //indicator = GetComponentInChildren<SphereCollider>().gameObject;
        enemyState = EnemyStates.Patrolling;
        player = FindObjectOfType<EasySurvivalScripts.PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update() {
        if(lastState != enemyState) {
            updateState();
        }

        if(enemyState == EnemyStates.Chasing) {
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
            transform.LookAt(player.transform.position);
        } else if(enemyState == EnemyStates.Suspicious) {
            indicator.GetComponent<MeshRenderer>().material.color = Color.blue;
        } else if(enemyState == EnemyStates.Searching) {
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        } else {
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        if(enemyState != EnemyStates.Patrolling) {
            timeoutSearch();
        }
    }

    //Mostly deals with the indicator color
    void updateState() {
        lastState = enemyState;
    }

    void look() {
        if(visCone.GameObjectIntoCone.Contains(player)) {
            alert(EnemyStates.Chasing);
        }
    }

    void timeoutSearch() {
        searchTimeout -= Time.deltaTime;
        if(searchTimeout <= 0) {
            enemyState = EnemyStates.Patrolling;
        }
    }

    public void hearSound(float volume, Vector3 location) {
        if(volume > 0.6) {
            enemyState = EnemyStates.Chasing;
            searchTimeout = 6f;
        } else if(volume > 0.3) {
            enemyState = EnemyStates.Suspicious;
            searchTimeout = 6f;

            transform.LookAt(location);
        }
    }

    public void alert(EnemyStates state) {
        //Debug.Log(GetInstanceID() + " heard you! Volume: " + volume);
        
    }
}
