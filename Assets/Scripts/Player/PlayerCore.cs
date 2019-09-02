using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts {
    public enum PlayerStates {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public enum MenuStates {
        None,
        Pause,
        Inv
    }

    public class PlayerCore : MonoBehaviour {
        public PlayerCamera Cam;
        public PlayerMovement Move;
        public PlayerInv Inv;
        public PlayerInteraction Use;

        // Start is called before the first frame update
        void Start() {
            Cam = GetComponentInChildren<PlayerCamera>();
            Move = GetComponentInChildren<PlayerMovement>();
            Inv = GetComponentInChildren<PlayerInv>();
            Use = GetComponentInChildren<PlayerInteraction>();

        }

        // Update is called once per frame
        void Update() {

        }
    }
}