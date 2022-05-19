using UnityEngine;
using TMPro;

/// <summary>
/// Handles timer feature independantly
/// </summary>
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textObject; // object to display the timer text
    public static string currentString { get; private set; }
    private float currentTime; // current time since last reset
    private bool isActive = true; // to stop the timer when the game is done

    private void OnEnable()
    {
        GameManager.GameOverEvent.AddListener(StopTimer);
        GameManager.GameStartEvent.AddListener(StartTimer);
    }

    private void OnDisable()
    {
        GameManager.GameOverEvent.RemoveListener(StopTimer); 
        GameManager.GameStartEvent.RemoveListener(StartTimer);
    }

    private void Update()
    {
        if (isActive)
        {
            currentTime += Time.deltaTime;
            SetDisplay();
        }
    }
    public void SetDisplay()
    {
        // Using TimeSpan here so that the formatting can be easily changed
        currentString = System.TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
        textObject.text = currentString;
    }

    public void StopTimer()
    {
        isActive = false;
    }

    public void StartTimer()
    {
        Reset();
        isActive = true;
    }

    public void Reset()
    {
        currentTime = 0f;
    }
}
