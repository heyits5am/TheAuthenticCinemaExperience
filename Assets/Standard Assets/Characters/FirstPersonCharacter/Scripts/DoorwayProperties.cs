using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayProperties : MonoBehaviour
{
    private float doorSpeed = 150;
    public float openRotation = 120;
    public bool startMove = false;
    public bool isOpen = true;
    private bool isMoving = false;

    public GameObject doorL;
    public GameObject doorR;

    // Start is called before the first frame update
    void Start() {
        Debug.Log(doorR.transform.localEulerAngles.y);
        if (isOpen) {
            doorL.transform.Rotate(0, openRotation - doorL.transform.localEulerAngles.y, 0);
            doorR.transform.Rotate(0, -doorR.transform.localEulerAngles.y - openRotation, 0);

        }
        else {
            doorL.transform.Rotate(0, - doorL.transform.localEulerAngles.y, 0);
            doorR.transform.Rotate(0, - doorR.transform.localEulerAngles.y, 0);
        }

        Debug.Log(doorR.transform.localEulerAngles.y);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (startMove) {
            isOpen = !isOpen;
            isMoving = true;
            startMove = false;
            doorL.gameObject.layer = 10;
            doorR.gameObject.layer = 10;
        }
        if (isMoving) {
            Swing();

        }
    }
    private float currentStep;
    void Swing() {
        currentStep = Time.fixedDeltaTime * doorSpeed;
        if (isOpen) {
            if (doorL.transform.localEulerAngles.y + currentStep >= openRotation) {
                Debug.Log("O L1: " + doorL.transform.localEulerAngles.y);
                Debug.Log("O R1: " + doorR.transform.localEulerAngles.y);
                doorL.transform.Rotate(0, openRotation - doorL.transform.localEulerAngles.y, 0);
                doorR.transform.Rotate(0, -doorR.transform.localEulerAngles.y - openRotation, 0);
                Debug.Log("O L2: " + doorL.transform.localEulerAngles.y);
                Debug.Log("O R2: " + doorR.transform.localEulerAngles.y);
                isMoving = false;
                doorL.gameObject.layer = 0;
                doorR.gameObject.layer = 0;
            }
            else {
                doorL.transform.Rotate(0, currentStep, 0);
                doorR.transform.Rotate(0, -currentStep, 0);
            }
        }
        else {
            if (doorL.transform.localEulerAngles.y - currentStep <= 0.01) {
                Debug.Log("C L1: " + doorL.transform.localEulerAngles.y);
                Debug.Log("C R1: " + doorR.transform.localEulerAngles.y);
                doorL.transform.Rotate(0, -doorL.transform.localEulerAngles.y, 0);
                doorR.transform.Rotate(0, -doorR.transform.localEulerAngles.y, 0);
                Debug.Log("C L1: " + doorL.transform.localEulerAngles.y);
                Debug.Log("C R1: " + doorR.transform.localEulerAngles.y);
                isMoving = false;
                doorL.gameObject.layer = 0;
                doorR.gameObject.layer = 0;
            }
            else {
                doorL.transform.Rotate(0, -currentStep, 0);
                doorR.transform.Rotate(0, currentStep, 0);
            }
        }
    }
}
