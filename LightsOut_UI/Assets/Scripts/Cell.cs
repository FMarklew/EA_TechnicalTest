using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles cell display and tracks if the cell is active (on) or not
/// </summary>
public class Cell : MonoBehaviour
{
    public GameObject displayImage; // Image to disable when the cell is not active
    public bool isActive { get; private set; } = false;
    private int iCoord;
    private int jCoord;

    public void Init(int i, int j)
    {
        iCoord = i;
        jCoord = j;
        displayImage.SetActive(isActive);
    }

    public void Toggle()
    {
        isActive = !isActive;
        displayImage.SetActive(isActive);
    }

    /// <summary>
    /// Called from button, and also by simulated clicks in GameManager
    /// </summary>
    public void OnClick()
    {
        GameManager.OnClickedCoordinates(iCoord, jCoord);
    }

    public void SetState(bool isActive)
    {
        this.isActive = isActive;
        displayImage.SetActive(isActive);
    }
}
