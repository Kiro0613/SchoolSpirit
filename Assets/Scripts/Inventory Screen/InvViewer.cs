using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvViewer : MonoBehaviour {
    Transform[] items;
    public float spinSpeed;

    // Start is called before the first frame update
    void Start() {
        items = GetComponentsInChildren<Transform>();
        updateItemList();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(items.Length);
        foreach(Transform item in items) {
            Vector3 target = item.eulerAngles;
            target.y = Mathf.MoveTowardsAngle(target.y, target.y + 10, Time.deltaTime * spinSpeed);
            item.eulerAngles = Vector3.MoveTowards(item.eulerAngles, target, Time.deltaTime * spinSpeed);
        }
    }

    public void updateItemList() {
        items = GetComponentsInChildren<Transform>(true);
        Transform[] newItems = new Transform[items.Length - 1];
        for(int i = 1; i < items.Length; i++) {
            newItems[i - 1] = items[i];
        }
        items = newItems;
    }
}
