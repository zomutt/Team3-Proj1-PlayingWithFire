using UnityEngine;
using UnityEngine.InputSystem;

namespace _1A_Scripts.Player
{
    public class InvectorCameraInput : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 0.1f;   // Matches the legacy "Mouse X/Y" axis sensitivity so Invector's own xMouseSensitivity/yMouseSensitivity fields stay meaningful
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoomDistance = 1f;
        [SerializeField] private float maxZoomDistance = 6f;

        private vThirdPersonCamera tpCamera;

        private void Start()
        {
            tpCamera = FindFirstObjectByType<vThirdPersonCamera>();
        }

        private void Update()
        {
            if (!tpCamera || Mouse.current == null) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;
            tpCamera.RotateCamera(mouseDelta.x, mouseDelta.y);

            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll != 0f)
            {
                tpCamera.defaultDistance = Mathf.Clamp(tpCamera.defaultDistance - scroll * zoomSpeed, minZoomDistance, maxZoomDistance);
            }
        }
    }
}
