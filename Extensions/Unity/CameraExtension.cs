using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Canty
{
    public static class CameraExtension
    {
        /// <summary>
        /// Checks if the given Vector3 can be found in the camera's frustum.
        /// </summary>
        public static bool IsWorldPointInViewport(this Camera camera, Vector3 point)
        {
            Vector3 position = camera.WorldToViewportPoint(point);
            return position.x >= 0 && position.y >= 0 && position.x <= 1.0f && position.y <= 1.0f;
        }

        /// <summary>
        /// Gets the mouse position on the screen. Requires a raycast catcher with the given layer for it to work.
        /// </summary>
        /// <param name="layerToHit">Which layer the camera-to-screen raycast is expected to hit. Recommended to have only one item with this layer, a flat plane perpendicular to the camera.</param>
        /// <param name="clamp">If the value of the mouse should be clamped to the sides of the screen.</param>
        public static bool MouseToScreenPosition(this Camera camera, ref RaycastHit hit, LayerMask layerToHit, Core.MouseToScreenReturnTypes returnType = Core.MouseToScreenReturnTypes.Clamped, float gameViewWidthPercent = 1.0f, float gameViewHeightPercent = 1.0f)
        {
            Vector2 mousePos = Input.mousePosition;
#if UNITY_EDITOR
            Vector2 gameView = Handles.GetMainGameViewSize();
#else
            Vector2 gameView = new Vector2(Screen.width, Screen.height);
#endif
            switch(returnType)
            {
                case Core.MouseToScreenReturnTypes.Clamped:
                    mousePos.x = Mathf.Clamp(mousePos.x, gameView.x * (1.0f - gameViewWidthPercent), gameView.x - gameView.x * (1.0f - gameViewWidthPercent));
                    mousePos.y = Mathf.Clamp(mousePos.y, gameView.y * (1.0f - gameViewHeightPercent), gameView.y - gameView.y * (1.0f - gameViewHeightPercent));
                    break;

                case Core.MouseToScreenReturnTypes.Limited:
                    if (mousePos.x < gameView.x * (1.0f - gameViewWidthPercent) || mousePos.x > gameView.x - gameView.x * (1.0f - gameViewWidthPercent) ||
                        mousePos.y < gameView.y * (1.0f - gameViewHeightPercent) || mousePos.y > gameView.y - gameView.y * (1.0f - gameViewHeightPercent))
                    {
                        return false;
                    }
                    break;
            }

            Ray ray = camera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0.0f));
            return Physics.Raycast(ray, out hit, layerToHit);
        }
    }
}

namespace Canty.Core
{
    /// <summary>
    /// Various ways the MouseToScreenPosition function can handle its data.
    /// </summary>
    public enum MouseToScreenReturnTypes
    {
        Free,       // Mouse can leave the screen as long as there is a raycast catcher to get the ray.
        Clamped,    // Mouse value is clamped to the sides of the screen if the mouse is outside.
        Limited     // Function returns false if the mouse leaves the screen.
    }
}