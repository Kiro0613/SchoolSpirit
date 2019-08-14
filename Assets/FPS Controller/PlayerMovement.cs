using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScripts {
    public enum PlayerStates {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public class PlayerMovement : MonoBehaviour {
        public PlayerStates playerStates;

        public bool canMove;

        [Header("Inputs")]
        public string HorizontalInput = "Horizontal";
        public string VerticalInput = "Vertical";
        public string RunInput = "Run";
        public string JumpInput = "Jump";

        [Header("Player Motor")]
        [Range(1f, 15f)]
        public float walkSpeed;
        [Range(1f, 15f)]
        public float runSpeed;
        [Range(1f, 15f)]
        public float JumpForce;

        [Header("Audio")]
        public AudioClip footstepSound;
        public AudioClip landingSound;
        public Transform soundNode;
        private int stepCycle;
        public int stepCycleSize;
        private bool incrementStepCycle;
        private int nextStep;
        public int nextStepIncrement;
        private bool didJump;

        CharacterController characterController;

        // Use this for initialization
        void Start() {
            characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update() {
            //handle controller
            HandlePlayerControls();
        }

        private void FixedUpdate() {
            if(incrementStepCycle) {
                stepCycle += stepCycleSize;
                if(stepCycle >= nextStep) {
                    //playSound(footstepSound);
                    Instantiate(soundNode, transform.position, transform.rotation, null);
                    nextStep += nextStepIncrement;
                }
            }
        }

        void HandlePlayerControls() {
            float hInput = Input.GetAxisRaw(HorizontalInput);
            float vInput = Input.GetAxisRaw(VerticalInput);

            Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
            Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;

            float _speed = Input.GetButton(RunInput) ? runSpeed : walkSpeed;
                characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);

            if(characterController.isGrounded) {
                Jump();
            }

            //Managing Player States
            if(characterController.isGrounded) {
                if(didJump) {
                    Instantiate(soundNode, null);
                    didJump = false;
                }

                if(hInput == 0 && vInput == 0) {
                    playerStates = PlayerStates.Idle;
                    stepCycle = 0;
                    stepCycleSize = 0;
                    nextStepIncrement = 60;
                    nextStep = nextStepIncrement;
                    incrementStepCycle = false;
                } else {
                    if(_speed == walkSpeed) {
                        playerStates = PlayerStates.Walking;
                        stepCycleSize = 2;
                        incrementStepCycle = true;
                    } else {
                        playerStates = PlayerStates.Running;
                        stepCycleSize = 4;
                        incrementStepCycle = true;
                    }
                }
            } else {
                playerStates = PlayerStates.Jumping;
                stepCycle = 0;
                stepCycleSize = 0;
                nextStepIncrement = 60;
                nextStep = nextStepIncrement;
                incrementStepCycle = false;
            }
        }

        void Jump() {
            if(Input.GetButtonDown(JumpInput)) {
                StartCoroutine(PerformJumpRoutine());
                didJump = true;
            }
        }

        IEnumerator PerformJumpRoutine() {
            float _jump = JumpForce;

            do {
                characterController.Move(Vector3.up * _jump * Time.deltaTime);
                _jump -= Time.deltaTime;
                yield return null;
            }
            while(!characterController.isGrounded);
        }

        //private void playSound(AudioClip sound) {
        //    audioSource.clip = sound;
        //    audioSource.Play();
        //}

    }
}