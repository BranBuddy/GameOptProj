using UnityEngine;

[RequireComponent(typeof(CollisionDataRetriever), typeof(Rigidbody2D), typeof(Controller))]
public class Jump : MonoBehaviour
{
    private Controller _controller = null;
    [SerializeField, Range(0, 100)] private float _jumpHeight = 4f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0, 5f)] private float _downwardMovementMultipiler = 3f;
    [SerializeField, Range(0, 5f)] private float _upwardMovementMultipiler = 1.7f;
    [SerializeField, Range(0, .3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0, .3f)] private float _jumpBufferTime = 0.2f;

    private Rigidbody2D _body;
    private CollisionDataRetriever _ground;
    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale;
    private bool _isJumping;
    private bool _desiredJump;
    private bool _onGround;
    private float _coyoteCounter;
    private float _jumpBufferCounter;
    private float _jumpSpeed;
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<CollisionDataRetriever>();
        _controller = this.GetComponent<Controller>();

        _defaultGravityScale = 1;
    }

    void Update()
    {
        _desiredJump |= _controller.inputController.RetrieveJumpInput(this.gameObject);
    }

    private void FixedUpdate()
    {
        _onGround = _ground.GetOnGround();
        _velocity = _body.linearVelocity;

        if(_onGround && _body.linearVelocity.y <= 0)
        {
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;
            _isJumping = false;
        } 
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }

        if(_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if(!_desiredJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if(_jumpBufferCounter > 0)
        {
            JumpAction();
        }

        if(_body.linearVelocity.y > 0 && _controller.inputController.RetrieveJumpHoldInput(this.gameObject))
        {
            _body.gravityScale = _upwardMovementMultipiler;
        }
        else if(_body.linearVelocity.y < 0 || !_controller.inputController.RetrieveJumpHoldInput(this.gameObject))
        {
            _body.gravityScale = _downwardMovementMultipiler;
        }
        else
        {
            _body.gravityScale = _defaultGravityScale;
        }

        _body.linearVelocity = _velocity;
    }

    private void JumpAction()
    {
        if(_coyoteCounter > 0 || (_jumpPhase < _maxAirJumps && _isJumping))
        {
            if(_isJumping)
                _jumpPhase += 1;

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultipiler);

            _jumpBufferCounter = 0;
            _coyoteCounter = 0;
            _isJumping = true;

            if(_velocity.y > 0)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            _velocity.y += _jumpSpeed;
        }
    }
}
