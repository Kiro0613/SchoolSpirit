using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsableObjects {
    public class OpenDoor : MonoBehaviour {
        [Header("Usable Object Script")]
        public bool isOpen;
        public bool isMoving;
        public bool reverseOpenDirection;
        public float openAngle;
        float closedAngle;
        public float openSpeed;

        // Start is called before the first frame update
        void Start() {
            closedAngle = transform.eulerAngles.y;
        }

        // Update is called once per frame
        void Update() {
            if(isMoving) {
                float currentRotation = transform.eulerAngles.y;
                float targetAngle = isOpen == false ? closedAngle : reverseOpenDirection ? closedAngle - openAngle : closedAngle + openAngle;

                if(Mathf.Abs(Mathf.DeltaAngle(currentRotation, targetAngle)) < 0.01) {
                    isMoving = false;
                }

                transform.eulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(currentRotation, targetAngle, Time.deltaTime * openSpeed));
            }
        }

        public void Use() {
            isOpen = !isOpen;
            isMoving = true;
        }
    }
}
