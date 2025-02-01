// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.InputSystem;

// public class RestartGame : MonoBehaviour
// {
//     public InputAction restartAction; // Reference to the Input Action

//     private void OnEnable()
//     {
//         restartAction.Enable();
//         restartAction.performed += OnRestart; // Listen for the input action
//     }

//     private void OnDisable()
//     {
//         restartAction.performed -= OnRestart; // Stop listening when disabled
//         restartAction.Disable();
//     }

//     private void OnRestart(InputAction.CallbackContext context)
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the game
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RestartGame : MonoBehaviour
{
    public InputAction restartAction; // Assignable input for opening confirmation
    public InputAction confirmAction; // Assignable input for confirming (e.g., A)
    public InputAction cancelAction; // Assignable input for canceling (e.g., B)
    
    public GameObject confirmationPanel; // Assign your UI panel in Inspector

    private void OnEnable()
    {
        restartAction.Enable();
        restartAction.performed += ShowConfirmationPanel;
        
        confirmAction.Enable();
        confirmAction.performed += ConfirmRestart;
        
        cancelAction.Enable();
        cancelAction.performed += CancelRestart;
    }

    private void OnDisable()
    {
        restartAction.performed -= ShowConfirmationPanel;
        restartAction.Disable();
        
        confirmAction.performed -= ConfirmRestart;
        confirmAction.Disable();
        
        cancelAction.performed -= CancelRestart;
        cancelAction.Disable();
    }

    private void ShowConfirmationPanel(InputAction.CallbackContext context)
    {
        confirmationPanel.SetActive(true); // Show confirmation panel
    }

    private void ConfirmRestart(InputAction.CallbackContext context)
    {
        if (confirmationPanel.activeSelf) // Only restart if panel is open
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void CancelRestart(InputAction.CallbackContext context)
    {
        if (confirmationPanel.activeSelf) // Only close panel if it's open
        {
            confirmationPanel.SetActive(false);
        }
    }
}
