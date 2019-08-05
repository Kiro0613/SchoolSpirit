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

        [Header("TPS Camera Settings")]
        public Vector3 TPS_CameraOffset;
        public Vector2 TPS_MinMaxAngles;

        [Header("Leaning")]
        public string leanInput = "Lean";
        public float zInput;
        public float leanSpeed;
        public float leanAngle;

        Transform FPSController;
        float xClamp;
        Vector3 camMoveLoc;
        Transform _fpsCameraHelper;
        Transform _tpsCameraHelper;

        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            xClamp = 0;
            FPSController = GetComponentInParent<PlayerMovement>().transform;
        }

        // Use this for initialization
        void Start() {
        }

        void Add_FPSCamPositionHelper() {
            _fpsCameraHelper = new GameObject().transform;
            _fpsCameraHelper.name = "_fpsCameraHelper";
            _fpsCameraHelper.localPosition = Vector3.zero;
        }


        void Add_TPSCamPositionHelper() {
            _tpsCameraHelper = new GameObject().transform;
            _tpsCameraHelper.name = "_tpsCameraHelper";
            _tpsCameraHelper.SetParent(FPSController);
            _tpsCameraHelper.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update() {
            RotateCamera();

            zInput = Input.GetAxisRaw(leanInput);
            if(zInput != 0 || transform.eulerAngles.z != 0) {
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    20 * zInput
                ), leanSpeed * Time.deltaTime);
            }
        }

        void RotateCamera() {
            float step = leanSpeed * Time.deltaTime;

            float mouseX = Input.GetAxis(MouseXInput) * (mouseSensitivity * Time.deltaTime);
            float mouseY = Input.GetAxis(MouseYInput) * (mouseSensitivity * Time.deltaTime);
            Vector3 eulerRotation = transform.eulerAngles;

            xClamp += mouseY;

            if(cameraPerspective == CameraPerspective.FirstPerson) {
                xClamp = Mathf.Clamp(xClamp, FPS_MinMaxAngles.x, FPS_MinMaxAngles.y);
            } else {
                xClamp = Mathf.Clamp(xClamp, TPS_MinMaxAngles.x, TPS_MinMaxAngles.y);
            }

            eulerRotation.x = -xClamp;
            transform.eulerAngles = eulerRotation;
            FPSController.Rotate(Vector3.up * mouseX);
        }

        private void OnDrawGizmosSelected() {
            if(_fpsCameraHelper) {
                Gizmos.DrawWireSphere(_fpsCameraHelper.position, 0.1f);
            }

            Gizmos.color = Color.green;

            if(_tpsCameraHelper) {
                Gizmos.DrawWireSphere(_tpsCameraHelper.position, 0.1f);
            }
        }

        private void lean() {

        }
    }
}