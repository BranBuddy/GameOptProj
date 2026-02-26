using UnityEngine;

[RequireComponent(typeof(CollisionDataRetriever), typeof(Rigidbody2D), typeof(Controller))]
public class WallInteractor : MonoBehaviour
{

    public bool WallJumping { get; private set; }


    [Header("Wall Slide")]
    [SerializeField, Range(0.1f, 5)] private float _wallSlideMaxSpeed = 2f;
    [SerializeField, Range(0.05f, .5f)] private float _wallStickTime = 0.25f;

    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(18f, 12f);

    private CollisionDataRetriever _collisionData;
    private Rigidbody2D _body;
    private Vector2 _velocity;
    private Controller _controller;

    private bool _onWall, _onGround, _desiredJump;
    private float _wallDirX, _wallStickCounter;

    void Start()
    {
        _collisionData = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();
        _controller = this.GetComponent<Controller>();
    }

    void Update()
    {
        if(_onWall && !_onGround)
            _desiredJump |= _controller.inputController.RetrieveJumpInput(this.gameObject);
    }

    void FixedUpdate()
    {
        _velocity = _body.linearVelocity;
        _onWall = _collisionData.onWall;
        _onGround = _collisionData.onGround;
        _wallDirX = _collisionData.ContactNormal.x;

        if(_collisionData.onWall && !_collisionData.onGround && !WallJumping)
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

            if(_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity.y = -_wallSlideMaxSpeed;
            }
        }

        if((_onWall && _velocity.x == 0) || _onGround)
        {
            WallJumping = false;
        }

        if (_desiredJump)
        {
            float jumpDir = 0f;

            if (_collisionData != null && _collisionData.ContactPoints != null && _collisionData.ContactPoints.Count > 0)
            {
                Vector2 contact = _collisionData.ContactPoints[0];
                jumpDir = (transform.position.x < contact.x) ? 1f : -1f;
            }
            if (jumpDir == 0) jumpDir = 1f; // Final fallback
            Debug.Log($"Wall Jump Direction: {jumpDir}");
            _velocity = new Vector2(-jumpDir * _wallJumpBounce.x, _wallJumpBounce.y);
            WallJumping = true;
            _desiredJump = false;
        }

        _body.linearVelocity = _velocity;
    }

    private void ChangeWallPropertyBasedOnType()
    {
        if(_collisionData.wallType == WallType.Sticky)
        {
            _wallSlideMaxSpeed = 0.5f; // Example: Increase stick time for sticky walls
        }
        else if(_collisionData.wallType == WallType.Bouncy)
        {
            _desiredJump = true;
            // Implement bouncy wall behavior
        }
        else if(_collisionData.wallType == WallType.Unjumpable)
        {
            _desiredJump = false; // Prevent jumping off unjumpable walls
            _wallSlideMaxSpeed = 3f; // Example: Increase slide speed for unjumpable walls
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
