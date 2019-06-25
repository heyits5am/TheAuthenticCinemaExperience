using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIndicator : MonoBehaviour {

    public Texture FocusTexture;
    public Texture closeUpTexture;
    public bool closeUp = false;
    public bool focusOn = true;
    public float imageSize = 0.75f;
    private float scalar;
    private float xMin;
    private float yMin;
    private float xCentre = Screen.width / 2;
    private float yCentre = Screen.height / 2;
    private Vector3 fwd;
    private Ray lookRay;
    public RaycastHit hit;
    public GameObject currentObject = null;
    private int armLength = 3;
    public int currentAction = 0;

    public enum Action
    {
        empty,
        hold,
        sit,
        view,
        replace
    }

    void OnGUI() {
        xCentre = Screen.width / 2;
        yCentre = Screen.height / 2;
        if (focusOn) {
            xMin = xCentre - (FocusTexture.width / 2);
            yMin = yCentre - (FocusTexture.height / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, FocusTexture.width, FocusTexture.height), FocusTexture);
        }
        if (closeUp) {
            scalar = System.Math.Min((float)Screen.width / (float)closeUpTexture.width, (float)Screen.height / (float)closeUpTexture.height);
            scalar = scalar * imageSize;
            xMin = xCentre - (closeUpTexture.width * scalar / 2);
            yMin = yCentre - (closeUpTexture.height * scalar / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, closeUpTexture.width * scalar, closeUpTexture.height * scalar), closeUpTexture);
        }

        fwd = transform.TransformDirection(Vector3.forward);
        lookRay = new Ray(transform.position, fwd);
        if (Physics.Raycast(lookRay, out hit, armLength)) {
            switch (hit.collider.tag)
            {
                case "holdable":
                    CenteredTextBox("Pick Up");
                    currentAction = (int) Action.hold;
                    currentObject = hit.collider.gameObject;
                    break;
                case "chair":
                    CenteredTextBox("Sit");
                    currentAction = (int) Action.sit;
                    currentObject = hit.collider.gameObject;
                    break;
                case "View":
                    CenteredTextBox("View");
                    currentAction = (int) Action.view;
                    currentObject = hit.collider.gameObject;
                    break;
                case "Replace":
                    currentAction = (int)Action.replace;
                    currentObject = hit.collider.gameObject;
                    break;
                default:
                    currentAction = (int)Action.empty;
                    currentObject = null;
                    break;
            }
        }
        else
        {
            currentAction = (int)Action.empty;
            currentObject = null;
        }
        Debug.Log(currentAction);
    }
    private void CenteredTextBox(string text) {
        GUI.Box(new Rect(xCentre - 35, 20 + yCentre, 70, 25), text);
    }
}
