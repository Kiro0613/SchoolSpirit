using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public enum State {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public enum Gui {
        None,
        Pause,
        Inv
    }

    public class Core : MonoBehaviour {
        public Cam Cam;
        public Move Move;
        public Inv Inv;
        public Use Use;

        // Start is called before the first frame update
        void Start() {
            Cam = GetComponentInChildren<Cam>();
            Move = GetComponentInChildren<Move>();
            Inv = GetComponentInChildren<Inv>();
            Use = GetComponentInChildren<Use>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}