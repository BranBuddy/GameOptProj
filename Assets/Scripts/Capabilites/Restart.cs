using UnityEngine;
using UnityEngine.InputSystem;

public class Restart : MonoBehaviour
{
    public Vector3 _restartPosition;
    [SerializeField] private InputActionReference _restartAction;
    private Controller _controller = null;
    private Vector3 _initialPosition;
    private bool _restart;
    public static Restart instance;

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
        if(!_controller.enabled)
            return;
        this.transform.position = _restartPosition != Vector3.zero ? _restartPosition : _initialPosition;
    }

    private void RestartLevel(InputAction.CallbackContext context)
    {
        Reset();
    }
}
