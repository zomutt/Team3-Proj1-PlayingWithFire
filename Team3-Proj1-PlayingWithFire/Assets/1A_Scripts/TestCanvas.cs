using UnityEngine;
using UnityEngine.InputSystem;

public class TestCanvas : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.f11Key.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
