using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    private LookIndicator lookIndicator;
    private GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("MainCamera");
        lookIndicator = player.GetComponent<LookIndicator>();
    }

    // Update is called once per frame
    void Update() {
        if (lookIndicator.currentAction == (int)LookIndicator.Action.door && Input.GetButtonDown("Interact")) {
            lookIndicator.currentObject.transform.parent.gameObject.GetComponent<DoorwayProperties>().startMove = true;
        }
    }

}
