using UnityEngine;
using TMPro;

/// <summary>
/// Handler for Game Over logic
/// </summary>
public class GameOverHandler : MonoBehaviour
{
    public GameObject GameBoard; // board to disable when game over
    public GameObject GameControls; // bottom buttons to disable when game over
    public GameObject GameOverOverlay; // overlay to show when game over

    private void OnEnable()
    {
        GameManager.GameStartEvent.AddListener(ResetDisplays);
        GameManager.GameOverEvent.AddListener(GameOver);
    }

    private void OnDisable()
    {
        GameManager.GameStartEvent.RemoveListener(ResetDisplays);
        GameManager.GameOverEvent.RemoveListener(GameOver);

    }
    public void GameOver()
    {
        GameBoard.SetActive(false); 
        GameOverOverlay.SetActive(true);
        GameControls.SetActive(false);
    }

    public void ResetDisplays()
    {
        GameBoard.SetActive(true);
        GameOverOverlay.SetActive(false);
        GameControls.SetActive(true);
    }
}
