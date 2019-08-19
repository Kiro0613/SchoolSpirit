using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;

namespace UsableObjects {
    public class PickUpItem : MonoBehaviour {
        [Header("Usable Object Script")]
        public bool locked;
        public bool isHeld;
        public GameObject holder;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if(Input.GetKeyDown(KeyCode.P)) {
                //Debug.Log("Obj Pos: " + (transform.position));
            }
        }

        public void Use(GameObject caller) {
            
        }
    }
}
