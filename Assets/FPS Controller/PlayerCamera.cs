using System.Collections;
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
        public float cameraLean;

        [Header("Head Collision")]
        public SphereCollider head;

        Transform FPSController;
        float xClamp;
        public float zClamp;
        Vector3 camMoveLoc;
        Transform _fpsCameraHelper;
        Transform _tpsCameraHelper;

        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            xClamp = 0;
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
            zClamp = Mathf.MoveTowardsAngle(transform.eulerAngles.z, cameraLean * zInput, leanSpeed * Time.deltaTime);

            Vector3 headOffset = Vector3.zero;
            headOffset.x = Mathf.MoveTowards(transform.localPosition.x, -zClamp * 0.1f, leanSpeed * Time.deltaTime);
            headOffset.y = Mathf.MoveTowards(transform.localPosition.y, (0.1f * -Mathf.Abs(zInput)) + 1, (leanSpeed / 64) * Time.deltaTime);
            //headOffset.x = -zClamp * 0.1f;
            //headOffset.y = (-0.5f * zInput) + 1;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, headOffset, leanSpeed * Time.deltaTime);

            eulerRotation.z = zClamp;
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
    }
}