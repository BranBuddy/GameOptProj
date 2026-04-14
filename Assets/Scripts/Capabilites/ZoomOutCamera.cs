/*
    Lets the player zoom out the camera to get a better view of the whole map
*/

using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class ZoomOutCamera : MonoBehaviour
{
    [SerializeField] private InputActionReference zoomOutAction; 
    [SerializeField] private CinemachineCamera zoomCamera; 
    private int defaultPriority = 1; 
    [SerializeField] private int zoomedOutPriority = 4; 

    private void OnEnable()
    {
        zoomOutAction.action.performed += ZoomOut; 
        zoomOutAction.action.canceled += ZoomIn; 
    }

    private void OnDisable()
    {
        zoomOutAction.action.performed -= ZoomOut; 
        zoomOutAction.action.canceled -= ZoomIn; 
    }

    private void Awake()
    {
        zoomCamera.Priority = defaultPriority;
    }

    private void ZoomOut(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return; 

        Debug.Log("Zoom out action performed"); 
    

        ToggleZoomCamera(); 
    }

    private void ZoomIn(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return;

        Debug.Log("Zoom in action performed"); 

        ToggleZoomCamera(); 
    }

    private void ToggleZoomCamera()
    {

        if (zoomCamera.Priority == defaultPriority)
        {
            zoomCamera.Follow = this.transform; 
            zoomCamera.Priority = zoomedOutPriority; 
        }
        else
        {
            zoomCamera.Follow = null; 
            zoomCamera.Priority = defaultPriority; 
        }
    }
}
