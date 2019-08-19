using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts {
    public class PlayerInv : MonoBehaviour {
        public List<GameObject> inv;
        public bool invOpen;
        public GameObject invViewer;
        public GameObject invHud;
        public float itemSpacing;
        public float slideSpeed;
        int activeObject;
        PlayerCamera playerCamera;
        Vector3 invDropSpot;
        Vector3 target;

        // Start is called before the first frame update
        void Start() {
            playerCamera = GetComponentInParent<PlayerCamera>();
            invOpen = false;
            target = Vector3.zero;
            activeObject = 0;
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
                //dropItem();
                Debug.Log(inv[activeObject].name);
            }

            if(Input.GetAxis("Item Select") != 0 && invOpen) {
                activeObject -= Input.GetAxis("Item Select") < 0 ? -1 : 1;
                activeObject = Mathf.Clamp(activeObject, 0, inv.Count - 1);
                Debug.Log(activeObject);
                target.x = itemSpacing * activeObject * -1;
            }

            invViewer.transform.localPosition = Vector3.MoveTowards(invViewer.transform.localPosition, target, Time.deltaTime * slideSpeed);
        }

        void dropItem() {
            //invDropSpot = playerCamera.invDropSpot;
            //activeObject = inv[inv.Count - 1];
            //activeObject.SetActive(true);
            //activeObject.transform.position = invDropSpot;
            //activeObject.transform.SetParent(null);
        }

        void openInv() {
            invOpen = true;             //Mark inv as OPEN
            invHud.SetActive(true);     //Make inv visible in HUD

            //Reset inv camera pos
            target = Vector3.zero;
            invViewer.transform.localPosition = Vector3.zero;

            invViewer.GetComponent<InvViewer>().updateItemList();

            //This is for instead of foreach b/c it needs i for item spacing
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