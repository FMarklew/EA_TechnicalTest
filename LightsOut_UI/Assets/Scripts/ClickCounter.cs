using UnityEngine;
using TMPro;

/// <summary>
/// Handler for click counter
/// </summary>
public class ClickCounter : MonoBehaviour
{
    private int clicksTotal; // current num clicks stored
    public TextMeshProUGUI clicksTotalDisplay; // Text Object to display num clicks
    private void OnEnable()
    {
        GameManager.OnClickedCoordinates += IncrementClicks;
        GameManager.GameStartEvent.AddListener(ResetClicks);
    }

    private void OnDisable()
    {
        GameManager.OnClickedCoordinates -= IncrementClicks;
        GameManager.GameStartEvent.RemoveListener(ResetClicks);
    }

    /// <summary>
    /// Arguments are just to facilitate the subscription
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public void IncrementClicks(int i, int j)
    {
        clicksTotal++;
        UpdateClicksDisplay();
    }

    public void ResetClicks()
    {
        clicksTotal = 0;
        UpdateClicksDisplay();
    }

    private void UpdateClicksDisplay()
    {
        clicksTotalDisplay.text = "Clicks: " + clicksTotal.ToString();
    }
}
