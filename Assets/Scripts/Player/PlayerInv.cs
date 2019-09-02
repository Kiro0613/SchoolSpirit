using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts {
    public class PlayerInv : MonoBehaviour {
        public List<GameObject> inv;
        public bool invOpen;
        public GameObject invViewer;
        public GameObject invHud;
        public float itemSpacing;
        public float slideSpeed;
        public bool holdingItem;
        int heldItem;
        PlayerCamera playerCamera;
        Vector3 invDropSpot;
        Vector3 target;


        // Start is called before the first frame update
        void Start() {
            playerCamera = GetComponent<PlayerCamera>();
            invOpen = false;
            target = Vector3.zero;
            holdingItem = false;
            heldItem = 0;
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetKeyDown(KeyCode.L)) {
                Debug.Log(heldItem);
            }

            if(Input.GetKeyDown(KeyCode.I) && inv.Count > 0) {
                if(invOpen) {
                    closeInv();
                } else {
                    openInv();
                }
            }

            if(invOpen) {
                if(Input.GetKeyDown(KeyCode.P)) {
                    dropItem();
                    //Debug.Log(inv[heldItem].name);
                }

                if(Input.GetAxis("Item Select") != 0) {
                    heldItem -= Input.GetAxis("Item Select") < 0 ? -1 : 1;
                    heldItem = Mathf.Clamp(heldItem, 0, inv.Count - 1);
                    Debug.Log(heldItem);
                    target.x = -itemSpacing * heldItem;
                    invHud.GetComponentInChildren<Text>().text = inv[heldItem].name;
                }
            }

            invViewer.transform.localPosition = Vector3.MoveTowards(invViewer.transform.localPosition, target, Time.deltaTime * slideSpeed);
        }

        public void holdItem() {
            holdingItem = true;
            invHud.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
            closeInv();
            invHud.SetActive(true);
            inv[heldItem].SetActive(true);
        }

        public void putAwayItem() {
            holdingItem = false;
            invHud.transform.localScale = Vector3.one;
            closeInv();
        }

        public void dropItem() {
            float dropDistance = Vector3.Distance(playerCamera.transform.position, playerCamera.invDropSpot);
            if(!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, dropDistance)) {
                inv[heldItem].GetComponent<Rigidbody>().useGravity = true;
                inv[heldItem].GetComponent<Rigidbody>().isKinematic = false;
                inv[heldItem].transform.position = playerCamera.invDropSpot;
                inv[heldItem].transform.rotation = playerCamera.transform.rotation;
                inv[heldItem].transform.SetParent(null);
                inv.Remove(inv[heldItem]);
                invViewer.GetComponent<InvViewer>().updateItemList(inv);
                closeInv();
            } else {
                Debug.Log("Too close to drop");
            }
        }

        public void openInv() {
            invOpen = true;             //Mark inv as OPEN
            invHud.SetActive(true);     //Make inv visible in HUD
            invHud.transform.localScale = Vector3.one;
            invHud.GetComponentInChildren<Text>().text = inv[heldItem].name;

            heldItem = heldItem >= inv.Count ? inv.Count - 1 : heldItem;

            invViewer.GetComponent<InvViewer>().updateItemList(inv);

            //Reset inv camera pos
            target = Vector3.zero;
            invViewer.transform.localPosition = Vector3.zero;
            target.x = -itemSpacing * heldItem;

            //This is for instead of foreach b/c it needs i for item spacing
            for(int i = 0; i < inv.Count; i++) {
                inv[i].SetActive(true);
                inv[i].GetComponent<Rigidbody>().useGravity = false;
                inv[i].GetComponent<Rigidbody>().isKinematic = true;
                inv[i].transform.eulerAngles = Vector3.zero;
                inv[i].transform.localPosition = new Vector3(i * itemSpacing, 0, 0);
            }

        }

        public void closeInv() {
            invOpen = false;
            invHud.SetActive(false);
            for(int i = 0; i < inv.Count; i++) {
                inv[i].SetActive(false);
            }
        }
    }
}