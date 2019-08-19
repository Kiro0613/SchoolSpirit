using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts {
    public class PlayerInv : MonoBehaviour {
        public List<GameObject> inv;
        PlayerCamera playerCamera;
        Vector3 invDropSpot;

        // Start is called before the first frame update
        void Start() {
            playerCamera = GetComponentInParent<PlayerCamera>();
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetKeyDown(KeyCode.P) && inv.Count > 0) {
                invDropSpot = playerCamera.invDropSpot;
                GameObject activeObject = inv[inv.Count - 1];
                activeObject.SetActive(true);
                activeObject.transform.position = invDropSpot;
                activeObject.transform.SetParent(null);

            }
        }
    }
}