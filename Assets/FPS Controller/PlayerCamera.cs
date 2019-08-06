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
        public float moveSpeed;
        public bool canLean;

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
            canLean = false;
            headOffset = Vector3.zero;
            FPSController = GetComponentInParent<PlayerMovement>().transform;
            head = GetComponent<SphereCollider>();
        }

        // Use this for initialization
        void Start() {
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
            if(canLean == false || zInput == 0) {
                headOffset.x = Mathf.MoveTowards(transform.localPosition.x, slideAmount * -zInput, slideAmount * 3 * Time.deltaTime);
                headOffset.y = Mathf.MoveTowards(transform.localPosition.y, (duckAmount * -Mathf.Abs(zInput)) + 1, duckAmount * 3 * Time.deltaTime);
                zClamp = Mathf.MoveTowardsAngle(transform.eulerAngles.z, leanAngle * zInput, leanAngle * 3 * Time.deltaTime);
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, headOffset, leanAngle * 3 * Time.deltaTime);

            eulerRotation.x = -xClamp;
            eulerRotation.z = zClamp;
            transform.eulerAngles = eulerRotation;
            FPSController.localPosition = Vector3.MoveTowards(FPSController.localPosition, Vector3.right * headOffset.x, slideAmount * 3 * Time.deltaTime);
            FPSController.Rotate(Vector3.up * mouseX);
        }

        private void OnDrawGizmosSelected() {
            if(_fpsCameraHelper) {
                Gizmos.DrawWireSphere(_fpsCameraHelper.position, 0.1f);
            }

            Gizmos.color = Color.green;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.name != "player") {
                canLean = true;
            }
            Debug.Log("Entered");
            Debug.Log(other.name);
        }

        private void OnTriggerExit(Collider other) {
            canLean = false;
            Debug.Log("Exited");
            Debug.Log(other.name);
        }
    }
}