/*
    Handles wall interactions for the player, including wall sliding and wall jumping. 
    It also includes different wall types that can affect the player's movement and jump properties.
*/

using UnityEngine;

[RequireComponent(typeof(CollisionDataRetriever), typeof(Rigidbody2D), typeof(Controller))]

public class WallInteractor : MonoBehaviour
{
    public bool WallJumping { get; private set; }

    [Header("Wall Slide")]
    [SerializeField, Range(0.1f, 5)] public float _wallSlideMaxSpeed = 2f;
    [SerializeField, Range(0.05f, .5f)] private float _wallStickTime = 0.25f;

    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(18f, 12f);
    [SerializeField] private AudioClip _wallJumpSFX;

    [Header("Wall Type SFX")]
    [SerializeField] private AudioClip _unjumpableWallSFX;
    [SerializeField] private AudioClip _stickyWallSFX;
    [SerializeField] private AudioClip _bouncyWallSFX;

    [Header("Wall Coyote Time")]
    [SerializeField] private float wallCoyoteTime = 0.15f; // Time after leaving wall to still allow wall jump
    private float wallCoyoteTimer = 0f;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private LayerMask wallLayer;

    private CollisionDataRetriever _collisionData;
    private Rigidbody2D _body;
    private Vector2 _velocity;
    private Controller _controller;

    private bool _onWall, _onGround, _desiredJump;
    private float _wallDirX, _wallStickCounter;
    private float wallJumpGraceTime = 0.15f;
    private float wallJumpGraceTimer = 0f;

    void Start()
    {
        _collisionData = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();
        _controller = this.GetComponent<Controller>();
    }

    void Update()
    {
        if((_onWall || wallCoyoteTimer > 0f) && !_onGround)
            _desiredJump |= _controller.inputController.RetrieveJumpInput(this.gameObject);
        // Wall jump grace timer countdown
        if (WallJumping)
            wallJumpGraceTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {

        _velocity = _body.linearVelocity;
        _onGround = _collisionData.onGround;

        // Only check wall in movement direction and only when airborne
        float moveInput = _controller.inputController.RetrieveMovementInput(this.gameObject);
        bool wallDetected = false;
        bool wallLeft = false;
        bool wallRight = false;
        if (!_onGround && Mathf.Abs(moveInput) > 0.01f)
        {
            Vector2 checkDir = moveInput < 0 ? Vector2.left : Vector2.right;
            Vector2 checkPos = (Vector2)transform.position + checkDir * wallCheckDistance * 0.5f;
            wallDetected = Physics2D.OverlapCircle(checkPos, wallCheckDistance * 0.5f, wallLayer);
            wallLeft = moveInput < 0 && wallDetected;
            wallRight = moveInput > 0 && wallDetected;
        }
        _onWall = wallLeft || wallRight || _collisionData.onWall;

        // Wall coyote time logic
        if (_onWall && !_onGround)
        {
            wallCoyoteTimer = wallCoyoteTime;
        }
        else
        {
            wallCoyoteTimer -= Time.fixedDeltaTime;
        }

        _wallDirX = _collisionData.ContactNormal.x;

        if(_onWall && !_onGround && !WallJumping)
        {
            if(_wallStickCounter > 0)
            {
                _velocity.x = 0;

                if(_controller.inputController.RetrieveMovementInput(this.gameObject) == _collisionData.ContactNormal.x)
                {
                    _wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }
            else
            {
                _wallStickCounter = _wallStickTime;
            }
        }

        if(_onWall)
        {
            ChangeWallPropertyBasedOnType();

            Slam slam = GetComponent<Slam>();

            if (!slam.IsSlamActive)
            {
                if(_velocity.y < -_wallSlideMaxSpeed)
                {
                    _velocity.y = -_wallSlideMaxSpeed;
                }
            }
        }

        if((_onWall && _velocity.x == 0) || _onGround || wallJumpGraceTimer <= 0f)
        {
            WallJumping = false;
        }

        if (_desiredJump && _onWall && !_onGround)
        {
            float jumpDir = 0f;
            if (_collisionData != null && _collisionData.ContactPoints != null && _collisionData.ContactPoints.Count > 0)
            {
                Vector2 contact = _collisionData.ContactPoints[0];
                jumpDir = (transform.position.x < contact.x) ? 1f : -1f;
            }
            else if (wallLeft)
            {
                jumpDir = 1f;
            }
            else if (wallRight)
            {
                jumpDir = -1f;
            }

            if (jumpDir != 0)
            {
                Debug.Log($"Wall Jump Direction: {jumpDir}");
                _velocity = new Vector2(-jumpDir * _wallJumpBounce.x, _wallJumpBounce.y);
                WallJumping = true;
                wallJumpGraceTimer = wallJumpGraceTime;
                _desiredJump = false;
                wallCoyoteTimer = 0f;
                SoundManager.Instance.sfxSource.PlayOneShot(_wallJumpSFX);
            }
        }

        _body.linearVelocity = _velocity;
    }

    private void ChangeWallPropertyBasedOnType()
    {
        if(_collisionData.wallType == WallType.Unjumpable)
        {
            _desiredJump = false; // Prevent jumping off unjumpable walls
            _wallSlideMaxSpeed = 3f; // Example: Increase slide speed for unjumpable walls
            SoundManager.Instance.sfxSource.PlayOneShot(_unjumpableWallSFX);
        }

        if(_collisionData.wallType == WallType.Sticky)
        {
            _wallSlideMaxSpeed = 0.5f; // Example: Increase stick time for sticky walls
            SoundManager.Instance.sfxSource.PlayOneShot(_stickyWallSFX);
        }
        else if(_collisionData.wallType == WallType.Bouncy)
        {
            Vector2 oldWallJumpBounce = _wallJumpBounce;
            _wallJumpBounce = new Vector2(oldWallJumpBounce.x * 2.5f, oldWallJumpBounce.y * 2.5f); // Example: Increase jump bounce for bouncy walls
            _desiredJump = true;
            SoundManager.Instance.sfxSource.PlayOneShot(_bouncyWallSFX);
            _wallJumpBounce = oldWallJumpBounce; // Reset to default after applying bounce effect
        }
        else
        {
            _wallSlideMaxSpeed = 2f; // Reset to default for non-sticky walls
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionData.EvaluteCollision(collision);

        if(_collisionData.onWall && !_collisionData.onGround && WallJumping)
        {
            _body.linearVelocity = Vector2.zero;
        }
    }
}
