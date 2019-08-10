﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScripts {
    public enum CameraPerspective {
        FirstPerson,
        ThirdPerson
    }

    public class PlayerCamera : MonoBehaviour {

        [Header("Input Settings")]
        public string MouseXInput;
        public string MouseYInput;

        [Header("Common Camera Settings")]
        public float mouseSensitivity;
        public CameraPerspective cameraPerspective;

        [Header("FPS Camera Settings")]
        public Vector3 FPS_CameraOffset;
        public Vector2 FPS_MinMaxAngles;

        [Header("Leaning")]
        public string leanInput = "Lean";
        public float zInput;
        public float leanAngle;
        public float slideAmount;
        public float duckAmount;
        public float distToWallL;
        public float distToWallR;
        //public float xOffsetAdjuster;
        //public float zOffsetAdjuster;
        public bool headInsideWall;

        Vector3 headOffset;

        [Header("Head Collision")]
        public SphereCollider head;

        Transform FPSController;
        float xClamp;
        public float zClamp;
        Vector3 camMoveLoc;
        Transform _fpsCameraHelper;

        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            xClamp = 0;
            headOffset = Vector3.zero;
            FPSController = GetComponentInParent<PlayerMovement>().transform;
            head = GetComponent<SphereCollider>();
        }

        private void FixedUpdate() {

        }

        void Add_FPSCamPositionHelper() {
            _fpsCameraHelper = new GameObject().transform;
            _fpsCameraHelper.name = "_fpsCameraHelper";
            _fpsCameraHelper.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update() {
            RotateCamera();
        }

        void RotateCamera() {
            float mouseX = Input.GetAxis(MouseXInput) * (mouseSensitivity * Time.deltaTime);
            float mouseY = Input.GetAxis(MouseYInput) * (mouseSensitivity * Time.deltaTime);
            Vector3 eulerRotation = transform.eulerAngles;

            xClamp += mouseY;
            xClamp = Mathf.Clamp(xClamp, FPS_MinMaxAngles.x, FPS_MinMaxAngles.y);

            zInput = Input.GetAxisRaw(leanInput);

            RaycastHit hit;
            if(zInput == 1 && Physics.Raycast(FPSController.position, -transform.right, out hit, slideAmount)) {
                distToWallL = Mathf.Round((hit.distance - 0.43f) / 0.57f * 1000) / 1000;
            } else {
                distToWallL = 1;
            }

            if(zInput == -1 && Physics.Raycast(FPSController.position, transform.right, out hit, slideAmount)) {
                distToWallR = Mathf.Round((hit.distance - 0.43f) / 0.57f * 1000) / 1000;
            } else {
                distToWallR = 1;
            }

            float slideAmountClamp = Mathf.Clamp(slideAmount * -zInput, -slideAmount * distToWallL, slideAmount * distToWallR);
            float duckAmountClamp = distToWallL < distToWallR ? duckAmount * distToWallL : duckAmount * distToWallR;
            float leanAngleClamp = Mathf.Clamp(leanAngle * zInput, -leanAngle * distToWallR, leanAngle * distToWallL);

            headOffset.x = Mathf.MoveTowards(transform.localPosition.x, slideAmountClamp, slideAmount * 3 * Time.deltaTime);
            headOffset.y = Mathf.MoveTowards(transform.localPosition.y, (duckAmountClamp * -Mathf.Abs(zInput)) + 1, duckAmount * 3 * Time.deltaTime);
            zClamp = Mathf.MoveTowardsAngle(transform.eulerAngles.z, leanAngleClamp, leanAngle * 3 * Time.deltaTime);
            //headOffsetX = Mathf.Clamp(headOffset.x, -slideAmount + (distToWall * slideAmount * Mathf.Abs(zInput)), slideAmount - (distToWall * slideAmount * Mathf.Abs(zInput) * 10));
            //zClamp = Mathf.Clamp(zClamp, -leanAngle + (distToWall / raycastLength) * leanAngle, leanAngle - (distToWall / raycastLength) * leanAngle);
            //headOffsetX = -slideAmount + (distToWall * slideAmount * Mathf.Abs(zInput));

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, headOffset, leanAngle * 3 * Time.deltaTime);

            eulerRotation.x = -xClamp;
            eulerRotation.z = zClamp;
            transform.eulerAngles = eulerRotation;
            transform.localPosition = headOffset;
            //FPSController.localPosition = Vector3.MoveTowards(FPSController.localPosition, new Vector3(headOffset.x, FPSController.localPosition.y, FPSController.localPosition.z), slideAmount * 3 * Time.deltaTime);
            FPSController.Rotate(Vector3.up * mouseX);
        }

        private void OnDrawGizmosSelected() {
            if(_fpsCameraHelper) {
                Gizmos.DrawWireSphere(_fpsCameraHelper.position, 0.1f);
            }

            Gizmos.color = Color.green;
        }

        private void OnTriggerEnter(Collider other) {
            headInsideWall = true;
            //Debug.Log("Entered");
            //Debug.Log(other.name);
        }

        private void OnTriggerExit(Collider other) {
            headInsideWall = false;
            //Debug.Log("Exited");
            //Debug.Log(other.name);
        }
    }
}