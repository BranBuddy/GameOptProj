using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Controller _controller = null;

    [Header("Dash Settings")]
    [SerializeField, Range(0, 20f)] private float _dashSpeed = 10f;
    [SerializeField, Range(0, 5f)] private float _dashDuration = 0.2f;
    public bool CanDash { get; private set; } = true;

    private CollisionDataRetriever _dataRetriever;

    private Rigidbody2D _body;
    private bool _isDashing;
    private float _dashTimeLeft;
    private float _lastDashTime;
    private bool _desiredDash;
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _controller = this.GetComponent<Controller>();
        _dataRetriever = this.GetComponent<CollisionDataRetriever>();
    }

    private void Update()
    {
        _desiredDash |= _controller.inputController.RetrieveDashInput(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (_desiredDash && CanDash)
        {
            StartDash();
        }

        if (_isDashing)
        {
            if (_dashTimeLeft > 0)
            {
                DashDirectionEffects();
                _dashTimeLeft -= Time.fixedDeltaTime;
            }
            else
            {
                EndDash();
            }
        }
    }

    private void StartDash()
    {
        _isDashing = true;
        _dashTimeLeft = _dashDuration;
        _lastDashTime = Time.time;
        _desiredDash = false;
    }

    private void EndDash()
    {
        CanDash = false;
        _isDashing = false;
        StartCoroutine(CheckGroundedAfterDash());
    }

    private IEnumerator CheckGroundedAfterDash()
    {
        while (!_dataRetriever.GetOnGround())
        {
            yield return null;
        }
        CanDash = true;
    }

    private void DashDirectionEffects()
    {
        if(GetDirectionHeld() == dirHeld.none)
        {
            _body.linearVelocity = Vector2.zero;
        }
        else if (GetDirectionHeld() == dirHeld.right)
        {
            _body.linearVelocity = new Vector2(transform.localScale.x * _dashSpeed, 0);
        }
        else
        {
            _body.linearVelocity = new Vector2(transform.localScale.x * -_dashSpeed, 0);
        }
    }

    private dirHeld GetDirectionHeld()
    {
        float horizontalInput = _controller.inputController.RetrieveMovementInput(this.gameObject);

        if (horizontalInput > 0)
        {
            return dirHeld.right;
        }
        else if (horizontalInput < 0)
        {
            return dirHeld.left;
        }
        else
        {
            return dirHeld.none;
        }
    }
}

public enum dirHeld
{
    left,
    right,
    none
}
