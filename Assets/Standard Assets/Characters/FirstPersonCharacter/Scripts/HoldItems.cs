using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class HoldItems : MonoBehaviour {
    
    public float throwSpeed = 10;
    public float inspectRotateSpeedX = 3;
    public float inspectRotateSpeedY = 3;

    //for all arrays, 0=left hand, 1=right hand
    private bool[] canHoldLR = new bool[] { true, true };
    public GameObject[] heldLR = new GameObject[] { null, null };
    private Transform[] guideLR = new Transform[] { null, null };
    private Transform inspectGuide;
    private bool inspecting = false;
    private bool throwing = false;

    //private bool replaceCheck;
    private GameObject player;
    private GameObject mainCamera;
    private CharacterController controller;
    private LookIndicator lookIndicator;
    //private GameObject[] replaceZones;

    enum Hand {
        left,
        right
    }

    //on initialisation, find player-related GameObjects
    void Start() {
        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        lookIndicator = mainCamera.GetComponent<LookIndicator>();
        //replaceZones = GameObject.FindGameObjectsWithTag("replace");
        guideLR[(int) Hand.left] = GameObject.FindWithTag("held guide L").transform;
        guideLR[(int) Hand.right] = GameObject.FindWithTag("held guide R").transform;
        inspectGuide = GameObject.FindWithTag("inspecting guide").transform;
    }

    
    void Update() {
        if (viewing) {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
                SetCloseUp(false);
                viewing = false;
                FreezePlayerMovement(false);
            }
        }
        if (heldLR[(int) Hand.left]) {
            //replaceCheck = Physics.Raycast(lookRay, out hit, armLength) && hit.collider.tag == "replace" && false; //hit.collider.gameObject.GetComponent<ReplaceProperties>().parentObj == held;
            //if (replaceCheck) {
            //    SetCanReplace(true);
            //}
            if (Input.GetButtonDown("Fire1")) {
                //if (replaceCheck) {
                //    Replace(hit.collider.gameObject);
                //}
                // else {
                ThrowItem((int) Hand.left);
                // }
                FreezePlayerMovement(false);
                throwing = true;
            }
            else {
                heldLR[(int) Hand.left].transform.position = guideLR[(int) Hand.left].position;
                inspecting = false;
                FreezePlayerMovement(false);
            }
        }
        if (heldLR[(int) Hand.right]) {
            if (Input.GetButtonDown("Fire2")) {
                //if (replaceCheck) {
                //    Replace(hit.collider.gameObject);
                //}
                // else {
                ThrowItem(Hand.right);
                // }
                FreezePlayerMovement(false);
                throwing = true;
            }
            else {
                heldLR[(int) Hand.right].transform.position = guideLR[(int) Hand.right].position;
                inspecting = false;
                FreezePlayerMovement(false);
            }
        }
        
        if (lookIndicator.currentAction == (int)LookIndicator.Action.hold && !throwing && !(heldLR[(int)Hand.left] && heldLR[(int)Hand.right])) {
            if (Input.GetButtonDown("Fire1") && !heldLR[(int)Hand.left]) {
                Pickup(lookIndicator.currentObject, Hand.left);
            }
            else if (Input.GetButtonDown("Fire2") && !heldLR[(int)Hand.right]) {
                Pickup(lookIndicator.currentObject, Hand.right);
            }
        }
        else if (lookIndicator.currentAction == (int) LookIndicator.Action.view && (Input.GetButtonDown("Fire1"))) {
            View(lookIndicator.hit.collider.gameObject);
        }

        throwing = false;
    }

    private Rigidbody objRB;
    private void Pickup(GameObject obj, Hand lr) {
        //We set the object parent to our guide empty object.
        obj.transform.SetParent(guideLR[(int) lr]);
        //Set gravity to false while holding it

        objRB = obj.GetComponent<Rigidbody>();
        objRB.useGravity = false;
        objRB.detectCollisions = false;
        objRB.freezeRotation = true;

        obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
        //We re-position the ball on our guide object 
        obj.transform.position = guideLR[(int) lr].position;
        heldLR[(int) lr] = obj;
        canHoldLR[(int) lr] = false;
        heldLR[(int) lr].layer = 8;
        //foreach (GameObject replaceZone in replaceZones) {
        //    replaceZone.layer = 10;
        //}
    }
    
    private void Inspect() {
        if (inspecting == false) {
            FreezePlayerMovement(true);
        }

        heldLR[(int) Hand.right].transform.position = inspectGuide.position;
        heldLR[(int) Hand.right].transform.Rotate(-transform.up  , Input.GetAxis("Mouse X") * inspectRotateSpeedX * Time.deltaTime);
        heldLR[(int) Hand.right].transform.Rotate(transform.right, Input.GetAxis("Mouse Y") * inspectRotateSpeedY * Time.deltaTime);
        
        inspecting = true;
    }

    private void ThrowItem(Hand lr) {
        LetGo(lr);

        heldLR[(int) lr].transform.parent = null;

        //reposition to good throwing position - tacoma moves it over time, TODO: clipping is still an issue
        heldLR[(int) lr].transform.position = inspectGuide.position;

        //Apply velocity on throwing
        heldLR[(int) lr].GetComponent<Rigidbody>().velocity = transform.forward * throwSpeed;

        //held is now empty
        heldLR[(int) lr] = null;
    }

    private void Replace(GameObject replace, Hand lr) {
        LetGo(lr);

        //Unparent the object
        heldLR[(int) lr].gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //held.transform.position = replace.GetComponent<ReplaceProperties>().parentPos;
        //held.transform.localRotation = replace.GetComponent<ReplaceProperties>().parentRot;
        heldLR[(int)lr] = null;
    }

    private void LetGo(Hand lr) {
        //returns to standard camera view
        heldLR[(int) lr].layer = 9;

        //Return physics to the object.
        objRB = heldLR[(int)lr].GetComponent<Rigidbody>();
        objRB.useGravity = true;
        objRB.detectCollisions = true;
        objRB.freezeRotation = false;
        heldLR[(int) lr].transform.parent = null;
        
        canHoldLR[(int) lr]=true;
         //foreach (GameObject replaceZone in replaceZones) {
         //   replaceZone.layer = 2;
        //}
    }
    bool viewing = false;
    private void View(GameObject obj) {

        //alternative solution, unfinished
        //obj.layer = 8;
        //Debug.Log("*glass breaking*");
        //viewing = Instantiate(obj, inspectGuide.position, inspectGuide.rotation);
        //viewing.transform.localScale = new Vector3(viewing.transform.localScale.x*3, viewing.transform.localScale.y * 3, 0);

        mainCamera.GetComponent<LookIndicator>().closeUpTexture = obj.GetComponent<SpriteRenderer>().sprite.texture;
        SetCloseUp(true);
        SetFocusOn(false);
        viewing = true;
        FreezePlayerMovement(true);
    }
    void FreezePlayerMovement(bool tf) {
        //player.GetComponent<FirstPersonController>().m_Freeze = tf;
    }
    void SetFocusOn(bool tf) {
        mainCamera.GetComponent<LookIndicator>().focusOn = tf;
    }
    void SetCloseUp(bool tf) {
        mainCamera.GetComponent<LookIndicator>().closeUp = tf;
    }
}