using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Locks mouse cursor inside the game window. 
    /// Locked by pressing left mouse button, unlocked by pressing the escape key.
    /// </summary>
    public class MouseLock : MonoBehaviour
    {
        private bool _isLocked = true;

        private void LateUpdate()
        {
			#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
                _isLocked = false;

            if (Input.GetMouseButtonDown(0))
                _isLocked = true;

            if (_isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
			#endif
        }
    }
}