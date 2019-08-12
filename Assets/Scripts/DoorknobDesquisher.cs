using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorknobDesquisher : MonoBehaviour {

    public float xScale;
    public float yScale;
    public float zScale;
    public bool useCurrentScale;

    // Start is called before the first frame update
    void Start(){
        Vector3 newScale;
        if(useCurrentScale) {
            newScale = transform.localScale;
        } else {
            newScale = new Vector3(xScale, yScale, zScale);
        }

        Transform originalParent = transform.parent;
        transform.SetParent(null);
        transform.localScale = newScale;
        transform.SetParent(originalParent);
    }
}
