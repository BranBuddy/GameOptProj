using UnityEngine;

public class ChangePlayerButton : CollisionInteraction
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject playerToActivate;
    [SerializeField] private GameObject playerToDeactivate;
    [SerializeField] private EmptyController emptyController;
    [SerializeField] private PlayerController playerController; 
    
    private Controller _playerActivateController;
    private Controller _playerDeactivateController;

    private void Start()
    {
        _playerActivateController = playerToActivate.GetComponent<Controller>();
        _playerDeactivateController = playerToDeactivate.GetComponent<Controller>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerActivateController.enabled = true;
            _playerActivateController.inputController = playerController;
            CameraManager.Instance.UpdateCameraTarget(playerToActivate);
            
            _playerDeactivateController.enabled = false;
            _playerDeactivateController.inputController = emptyController;
            
            
        }
    }
}
