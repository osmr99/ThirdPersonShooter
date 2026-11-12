using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class _OmarScript : MonoBehaviour
{
    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
            EditorApplication.ExitPlaymode();

        if (Keyboard.current.qKey.wasPressedThisFrame)
            EditorApplication.isPaused = !EditorApplication.isPaused;

        if (Keyboard.current.cKey.wasPressedThisFrame)
            SceneManager.LoadScene(0);

        //if (Keyboard.current.leftAltKey.isPressed)
        //Cursor.lockState = CursorLockMode.None;
        //else
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
