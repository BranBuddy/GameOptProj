using UnityEngine;

public class FloorInteractor : MonoBehaviour
{
    [Header("Floor Jump")]
    [SerializeField] private float _defaultFriction = 0.4f;
    [SerializeField] private float _stickyFriction = 1f;
    [SerializeField] private float _bouncyJumpMultiplier = 1.5f;

    private CollisionDataRetriever _collisionData;
    private Rigidbody2D _body;
    private Vector2 _velocity;
    private Controller _controller;
    private Jump _jump;
    private bool _onGround, _wasOnGround, _desiredJump;
    private float _originalJumpHeight;

    void Start()
    {
        _collisionData = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();
        _controller = GetComponent<Controller>();
        _jump = GetComponent<Jump>();
        if (_jump != null)
            _originalJumpHeight = _jump._jumpHeight;
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
            _desiredJump = true;
        }
        else if (_jump != null)
        {
            // Reset to default for non-bouncy floors
            _jump._jumpHeight = _originalJumpHeight;
        }
    }
}
