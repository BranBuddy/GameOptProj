using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(CollisionDataRetriever), typeof(Rigidbody2D), typeof(Controller))]
public class Move : MonoBehaviour
{
    private Controller _controller = null;
    [SerializeField, Range(0, 100)] private float _maxSpeed = 4f;
    [SerializeField, Range(0, 100)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0, 100)] private float _maxAirAcceleration = 20f;

    private Vector2 _direction, _desiredVelocity, _velocity;
    private Rigidbody2D _body;
    private CollisionDataRetriever _dataRetriever;

    private float _maxSpeedChange, _acceleration;
    private bool _onGround;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _dataRetriever = GetComponent<CollisionDataRetriever>();
        _controller = this.GetComponent<Controller>();
    }

    void Update()
    {
        _direction.x = _controller.inputController.RetrieveMovementInput(this.gameObject);
        _direction.y = _controller.inputController.RetrieveVerticalInput(this.gameObject);
        _desiredVelocity = new Vector2(_direction.x, _direction.y) * Mathf.Max(_maxSpeed - _dataRetriever.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        _onGround = _dataRetriever.GetOnGround();
        _velocity = _body.linearVelocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange);

        _body.linearVelocity = _velocity;
    }
}
