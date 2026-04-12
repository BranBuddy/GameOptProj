using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Controller _controller = null;

    [Header("Dash Settings")]
    [SerializeField, Range(0, 20f)] public float _dashSpeed = 10f;
    [SerializeField, Range(0, 5f)] private float _dashDuration = 0.2f;
    [SerializeField] private AudioClip _dashSFX;
    public bool CanDash { get; private set; } = true;

    private CollisionDataRetriever _dataRetriever;

    private Rigidbody2D _body;
    private bool _isDashing;
    private float _dashTimeLeft;
    private float _lastDashTime;
    private bool _desiredDash;
    private dirHeld _lastDirectionHeld = dirHeld.none;
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _controller = this.GetComponent<Controller>();
        _dataRetriever = this.GetComponent<CollisionDataRetriever>();
    }

    void Start()
    {
        StartCoroutine(GetLastDirectionHeldCoroutine());
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
            SoundManager.Instance.sfxSource.PlayOneShot(_dashSFX);
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

    private IEnumerator GetLastDirectionHeldCoroutine()
    {
        while (true)
        {
            GetDirectionHeld();
            yield return null;
        }
    }

    private IEnumerator CheckGroundedAfterDash()
    {
        while (!_dataRetriever.GetOnGround())
        {
            yield return null;
        }
        CanDash = true;
    }

    private void LastDirectionEffects()
    {
        if (_lastDirectionHeld == dirHeld.right)
        {
            _body.linearVelocity = new Vector2(transform.localScale.x * _dashSpeed, 0);
        }
        else
        {
            _body.linearVelocity = new Vector2(transform.localScale.x * -_dashSpeed, 0);
        }
    }

    private void DashDirectionEffects()
    {
        if(GetDirectionHeld() == dirHeld.none)
        {
           LastDirectionEffects();
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
            _lastDirectionHeld = dirHeld.right;
            return dirHeld.right;
        }
        else if (horizontalInput < 0)
        {
            _lastDirectionHeld = dirHeld.left;
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
