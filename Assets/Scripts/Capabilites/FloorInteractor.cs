using UnityEngine;

public class FloorInteractor : MonoBehaviour
{
    [Header("Floor Jump")]
    [SerializeField] private float _defaultFriction = 0.4f;
    [SerializeField] private float _stickyFriction = 1f;
    [SerializeField] private float _bouncyJumpMultiplier = 1.5f;

    [Header("Floor Type SFX")]
    [SerializeField] private AudioClip _stickyFloorSFX;
    [SerializeField] private AudioClip _bouncyFloorSFX;

    private CollisionDataRetriever _collisionData;
    private Rigidbody2D _body;
    private Vector2 _velocity;
    private Controller _controller;
    private Jump _jump;
    private Move _move;
    private bool _onGround, _wasOnGround, _desiredJump;
    private float _originalJumpHeight, _originalMaxSpeed, _originalMaxAcceleration, _originalAirAcceleration, _originalWallSlideMaxSpeed;

    void Start()
    {
        _collisionData = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();
        _controller = GetComponent<Controller>();
        _jump = GetComponent<Jump>();
        _move = GetComponent<Move>();
        if (_jump != null)
            _originalJumpHeight = _jump._jumpHeight;
        if (_move != null)
            _originalMaxSpeed = _move._maxSpeed;
        if (_move != null)
            _originalMaxAcceleration = _move._maxAcceleration;
        if (_move != null)
            _originalAirAcceleration = _move._maxAirAcceleration;
    }

    void Update()
    {
        if (_onGround)
            _desiredJump |= _controller.inputController.RetrieveJumpInput(this.gameObject);
    }

    void FixedUpdate()
    {
        _velocity = _body.linearVelocity;
        _wasOnGround = _onGround;
        _onGround = _collisionData.onGround;

        if (_onGround)
        {
            ChangeFloorPropertyBasedOnType();
        }
        else
        {
            // Reset friction to default when not on ground
            if (_body.sharedMaterial != null)
                _body.sharedMaterial.friction = _defaultFriction;
            if (_jump != null)
                _jump._jumpHeight = _originalJumpHeight;
        }

        // Automatic bounce: just landed on bouncy floor
        if (!_wasOnGround && _onGround && _collisionData.floorType == FloorType.Bouncy && _jump != null)
        {
            _jump._desiredJump = true;
        }

        if (_desiredJump && _onGround)
        {
            // Trigger jump by setting _desiredJump on Jump component
            if (_jump != null)
            {
                _jump._desiredJump = true;
                _desiredJump = false;
            }
        }
    }

    private void ChangeFloorPropertyBasedOnType()
    {
        if (_body.sharedMaterial != null)
        {
            if (_collisionData.floorType == FloorType.Sticky)
            {
                _body.sharedMaterial.friction = _stickyFriction;
            }
            else
            {
                _body.sharedMaterial.friction = _defaultFriction;
            }
        }

        if (_collisionData.floorType == FloorType.Bouncy && _jump != null)
        {
            // Temporarily increase jump height for bouncy floors
            _jump._jumpHeight = _originalJumpHeight * _bouncyJumpMultiplier;
            SoundManager.Instance.sfxSource.PlayOneShot(_bouncyFloorSFX); // Play bouncy floor sound effect
            _desiredJump = true;
        }
        else if (_jump != null)
        {
            // Reset to default for non-bouncy floors
            _jump._jumpHeight = _originalJumpHeight;
            _move._maxSpeed = _originalMaxSpeed;
            _move._maxAcceleration = _originalMaxAcceleration;
            _move._maxAirAcceleration = _originalAirAcceleration;
        }
        
        if (_collisionData.floorType == FloorType.Icy)
        {
            _move._maxSpeed *= 2.5f; // Increase max speed on icy floors
            _move._maxAcceleration *= 1.5f; // Decrease acceleration on icy floors
            _move._maxAirAcceleration *= 1.5f; // Decrease air acceleration on icy floors
        }
    }
}
