using UnityEngine;

public class Restart : MonoBehaviour
{
    private Controller _controller = null;
    private Vector3 _initialPosition;
    private bool _restart;

    void Awake()
    {
        _controller = this.GetComponent<Controller>();
        _initialPosition = this.transform.position;
    }

    void Update()
    {
        if (_restart |= _controller.inputController.RetrieveRestartInput(this.gameObject))
        {
            _restart = true;
        }
    }

    void FixedUpdate()
    {
        if (_restart)
        {
            RestartLevel();
            _restart = false;
        }
    }

    private void RestartLevel()
    {
        this.transform.position = _initialPosition;
    }
}
