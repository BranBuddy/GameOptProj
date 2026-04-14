/*
    This script handles the player's ability to reset to the last checkpoint. I
    t listens for a specific input action and when triggered, it resets the player's position to the last checkpoint and increases the death count for the player. 
    It also has a cooldown to prevent spamming the reset button.
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class Restart : MonoBehaviour
{
    public Vector3 _restartPosition;
    [SerializeField] private InputActionReference _restartAction;
    private Controller _controller = null;
    private Vector3 _initialPosition;
    public static Restart instance;
    private bool _isResetting = false;
    private float _resetCooldown = 0.2f; // seconds

    void Awake()
    {
        instance = this;
        _controller = this.GetComponent<Controller>();
        _initialPosition = this.transform.position;
        if (_restartPosition != Vector3.zero)
            _initialPosition = _restartPosition;
    }

    private void OnEnable()
    {
        _restartAction.action.performed += RestartLevel;
    }

    private void OnDisable()
    {
        _restartAction.action.performed -= RestartLevel;
    }


    public void Reset()
    {
        if (_isResetting || !_controller.enabled)
            return;
        _isResetting = true;
        this.transform.position = _restartPosition != Vector3.zero ? _restartPosition : _initialPosition;
        GameManager.Instance.IncreasePlayerDeathCount(GameManager.Instance.currentPlayer);
        Invoke(nameof(ResetCooldown), _resetCooldown);
    }

    private void ResetCooldown()
    {
        _isResetting = false;
    }

    private void RestartLevel(InputAction.CallbackContext context)
    {
        Reset();
    }
}
