using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts {
    public class PlayerInv : MonoBehaviour {
        public List<GameObject> inv;
        public bool invOpen;
        PlayerCamera playerCamera;
        Vector3 invDropSpot;
        public GameObject invViewer;
        public GameObject invHud;
        GameObject activeObject;
        public float itemSpacing;
        Vector3 target;
        public float slideSpeed;

        // Start is called before the first frame update
        void Start() {
            playerCamera = GetComponentInParent<PlayerCamera>();
            invOpen = false;
            target = Vector3.zero;
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetKeyDown(KeyCode.I)) {
                if(invOpen) {
                    closeInv();
                } else {
                    openInv();
                }
            }

            if(Input.GetKeyDown(KeyCode.P)) {
                dropItem();
            }

            if(Input.GetAxis("Item Select") != 0 && invOpen) {
                target.x += itemSpacing * Input.GetAxis("Item Select") * 10;
            }

            invViewer.transform.localPosition = Vector3.MoveTowards(invViewer.transform.localPosition, target, Time.deltaTime * slideSpeed);
        }

        void dropItem() {
            invDropSpot = playerCamera.invDropSpot;
            activeObject = inv[inv.Count - 1];
            activeObject.SetActive(true);
            activeObject.transform.position = invDropSpot;
            activeObject.transform.SetParent(null);
        }

        void openInv() {
            invOpen = true;
            invHud.SetActive(true);
            invViewer.transform.localPosition = Vector3.zero;
            target = Vector3.zero;
            invViewer.GetComponent<InvViewer>().updateItemList();
            for(int i = 0; i < inv.Count; i++) {
                inv[i].SetActive(true);
                inv[i].GetComponent<Rigidbody>().useGravity = false;
                inv[i].GetComponent<Rigidbody>().isKinematic = true;
                inv[i].transform.eulerAngles = Vector3.zero;
                inv[i].transform.localPosition = new Vector3(i * itemSpacing, 0, 0);
            }
        }

        void closeInv() {
            invOpen = false;
            invHud.SetActive(false);
            for(int i = 0; i < inv.Count; i++) {
                inv[i].SetActive(false);
            }
        }
    }
}