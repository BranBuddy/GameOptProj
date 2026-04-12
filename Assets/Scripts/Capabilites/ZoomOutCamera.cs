using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class ZoomOutCamera : MonoBehaviour
{
    [SerializeField] private InputActionReference zoomOutAction; // Reference to the input action for zooming out
    [SerializeField] private CinemachineCamera zoomCamera; // Reference to the Cinemachine camera to zoom out
    private int defaultPriority = 1; // Default priority for the zoom camera
    [SerializeField] private int zoomedOutPriority = 4; // Priority for the zoom camera when zoomed out

    private void OnEnable()
    {
        zoomOutAction.action.performed += ZoomOut; // Subscribe to the performed event of the input action
        zoomOutAction.action.canceled += ZoomIn; // Subscribe to the canceled event of the input action to toggle the zoom camera back when the action is released
    }

    private void OnDisable()
    {
        zoomOutAction.action.performed -= ZoomOut; // Unsubscribe from the performed event of the input action
        zoomOutAction.action.canceled -= ZoomIn; // Unsubscribe from the canceled event of the input action
    }

    private void Awake()
    {
        zoomCamera.Priority = defaultPriority; // Set the initial priority of the zoom camera to the default value
    }

    private void ZoomOut(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return; // Check if the current player is the one associated with this script, if not, return early

        Debug.Log("Zoom out action performed"); // Log a message when the zoom out action is performed
    

        ToggleZoomCamera(); // Toggle the zoom camera when the input action is performed
    }

    private void ZoomIn(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return; // Check if the current player is the one associated with this script, if not, return early

        Debug.Log("Zoom in action performed"); // Log a message when the zoom in action is performed

        ToggleZoomCamera(); // Toggle the zoom camera back when the input action is released
    }

    private void ToggleZoomCamera()
    {

        if (zoomCamera.Priority == defaultPriority)
        {
            zoomCamera.Follow = this.transform; // Set the follow target of the zoom camera to the current player
            zoomCamera.Priority = zoomedOutPriority; // Set priority to the zoomed out value to enable the zoom camera
        }
        else
        {
            zoomCamera.Follow = null; // Clear the follow target of the zoom camera
            zoomCamera.Priority = defaultPriority; // Set priority to the default value to disable the zoom camera
        }
    }
}
