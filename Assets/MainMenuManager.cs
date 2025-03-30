using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu; // Assign the MainMenu parent
    public GameObject startButton; // Assign the StartButton (for UI focus)
    public InputAction interactAction; // Assign the interact action

    void Start()
    {
        // Ensure menu is visible and game is paused at the start
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
        Time.timeScale = 0; // Pause the game while in menu
        Debug.Log("Game Paused: " + Time.timeScale);

        // Enable the interact action
        interactAction.Enable();
    }

    void Update()
    {
        // Check if the interact action was triggered
        if (interactAction.triggered)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false); // Hide menu

        // Debug log to ensure this part is triggered
        Debug.Log("Starting Game, unpausing...");

        Time.timeScale = 1; // Resume game
        Debug.Log("Game Unpaused: " + Time.timeScale);
    }

    private void OnDisable()
    {
        // Disable the interact action when the script is disabled
        interactAction.Disable();
    }
}
