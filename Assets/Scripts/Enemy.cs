using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    AIConeDetection visCone;
    // Start is called before the first frame update
    void Start()
    {
        visCone = GetComponent<AIConeDetection>();
    }

    // Update is called once per frame
    void Update(){
        Debug.Log(visCone.GameObjectIntoCone.Count);
    }
}
