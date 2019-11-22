﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Use : MonoBehaviour {
        public float lookDistance;
        public Cam playerCamera;
        bool holdingObject;
        GameObject objectHit;
        Inv playerInv;

        // Start is called before the first frame update
        void Start() {
            playerCamera = GetComponentInChildren<Cam>();
            playerInv = GetComponentInChildren<Inv>();
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown("Fire1")) {
                if(playerInv.invOpen) {
                    playerInv.dropItem();
                } else if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, lookDistance, 1 << 10)) {
                    if(hit.transform.CompareTag("Pickup")) {
                        Inv Daddy = GetComponentInChildren<Inv>();
                        Daddy.inv.Add(hit.transform.gameObject);
                        hit.transform.SetParent(Daddy.invViewer.transform);
                        hit.transform.localPosition = new Vector3(0, 0, 0);
                        hit.transform.gameObject.SetActive(false);
                    } else {
                        hit.transform.gameObject.SendMessage("Use", gameObject);
                    }
                }
            }

            if(Input.GetButtonDown("Fire2") && playerInv.invOpen) {
                playerInv.holdItem();
            }

            if(Input.GetButtonDown("Fire3")) {
                playerInv.putAwayItem();
            }

            if(Input.GetButtonDown("Cancel")) {
                if(playerInv.invOpen) {
                    playerInv.closeInv();
                }

                if(playerInv.holdingItem) {
                    playerInv.putAwayItem();
                }
            }
        }
    }
}
