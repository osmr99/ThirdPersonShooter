using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class _OmarScript : MonoBehaviour
{
    bool lockedMouse = true;
    private void FixedUpdate()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
            EditorApplication.ExitPlaymode();

        if (Keyboard.current.leftAltKey.isPressed)
            lockedMouse = false;
        else
            lockedMouse = true;

        if(lockedMouse)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
