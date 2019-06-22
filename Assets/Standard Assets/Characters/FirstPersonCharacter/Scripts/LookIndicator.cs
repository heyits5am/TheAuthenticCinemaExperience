using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIndicator : MonoBehaviour {

    public Texture FocusTexture;
    public Texture closeUpTexture;
    public bool closeUp = false;
    public bool focusOn = true;
    public bool canView;
    public bool canSit;
    public bool canPickup;
    public bool canReplace;
    public float imageSize = 0.75f;
    private float scalar;
    private float xMin;
    private float yMin;
    private float xCentre = Screen.width / 2;
    private float yCentre = Screen.height / 2;
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
        if (canView) {
            CenteredTextBox("View");
        }
        else if (canPickup) {
            CenteredTextBox("Pick Up");
        }
        else if (canReplace) {
            CenteredTextBox("Replace");
        }
        else if (canSit) {
            CenteredTextBox("Sit");
        }
    }
    private void CenteredTextBox(string text) {
        GUI.Box(new Rect(xCentre - 35, 20 + yCentre, 70, 25), text);
    }
}
