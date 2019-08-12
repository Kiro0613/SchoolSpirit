using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySurvivalScripts;

public class PlayerInteraction : MonoBehaviour {
    public float lookDistance;
    public PlayerCamera playerCamera;
    bool holdingObject;
    GameObject objectHit;

    // Start is called before the first frame update
    void Start(){
        playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetButtonDown("Fire1")) {
            if(Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit hit, lookDistance)) {
                objectHit = hit.transform.gameObject;
                objectHit.SendMessageUpwards("Activate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
