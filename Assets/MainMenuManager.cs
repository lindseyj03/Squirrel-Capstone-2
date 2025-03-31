//using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu; // Main Menu UI
    public GameObject startButton; // First button to be selected
    public CutsceneManager cutsceneManager; // Reference to CutsceneManager
    public InputAction interactAction; // Interact action

    void Start()
    {
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
        Time.timeScale = 0; // Pause game
        interactAction.Enable();
    }

    void Update()
    {
        if (interactAction.triggered)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false); // Hide main menu
        Time.timeScale = 0.75f; // Resume game now that cutscene starts
        cutsceneManager.StartCutsceneWithFade(); // Start cutscene and trigger the fade to black
    }

    //private IEnumerator ResumeAfterFade()
    //{
    //    // Wait until the fade has completed
    //    yield return new WaitForSeconds(5f); // Wait for fade duration (adjust as needed)

    //    // Now resume the game
    //    Time.timeScale = 1;
    //}

    private void OnDisable()
    {
        interactAction.Disable(); // Disable interact action when Main Menu is disabled
    }
}
