using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RestartGame : MonoBehaviour
{
    public InputAction restartAction; // Reference to the Input Action

    private void OnEnable()
    {
        restartAction.Enable();
        restartAction.performed += OnRestart; // Listen for the input action
    }

    private void OnDisable()
    {
        restartAction.performed -= OnRestart; // Stop listening when disabled
        restartAction.Disable();
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the game
    }
}
