using UnityEngine;

public class ChangePlayerButton : CollisionInteraction
{
    [SerializeField] private GameObject playerToActivate;
    [SerializeField] private GameObject playerToDeactivate;
    [SerializeField] private EmptyController emptyController;
    [SerializeField] private PlayerController playerController; 
    
    [SerializeField] private AudioClip _switchSFX;
    private Controller _playerActivateController;
    private Controller _playerDeactivateController;

    private void Start()
    {
        _playerActivateController = playerToActivate.GetComponent<Controller>();
        _playerDeactivateController = playerToDeactivate.GetComponent<Controller>();
        Debug.Log($"[ChangePlayerButton] Start: ActivateController={_playerActivateController} on {playerToActivate.name}, DeactivateController={_playerDeactivateController} on {playerToDeactivate.name}");
    }


    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Debug.Log($"[ChangePlayerButton] OnCollisionEnter2D with {collision.collider.name}, tag={collision.collider.tag}");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"[ChangePlayerButton] Player collision detected. Enabling {_playerActivateController} on {playerToActivate.name}, disabling {_playerDeactivateController} on {playerToDeactivate.name}.");
            _playerActivateController.enabled = true;
            _playerActivateController.inputController = playerController;
            SoundManager.Instance.sfxSource.PlayOneShot(_switchSFX); // Play the switch sound effect
            CameraManager.Instance.UpdateCameraTarget(playerToActivate);

            _playerDeactivateController.enabled = false;
            _playerDeactivateController.inputController = emptyController;

            GameManager.Instance.currentPlayer = playerToActivate;
        }
    }
}
