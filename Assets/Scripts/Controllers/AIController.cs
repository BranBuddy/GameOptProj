using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController", order = 2)]
public class AIController : InputController
{
    [Header("Interaction")]
    [SerializeField] private LayerMask _layerMask = -1;
    [Header("Ray")]
    [SerializeField] private float _bottomDistance = 1f;
    [SerializeField] private float _topDistance = 1f;
    [SerializeField] private float _xOffset = 1f;

    private RaycastHit2D _groundInfoBottom;
    private RaycastHit2D _groundInfoTop;
    private bool _isJumping = false;


    public override float RetrieveMovementInput(GameObject gameObject)
    {
        _groundInfoBottom = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y), Vector2.down, _bottomDistance, _layerMask);

        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y), Vector2.down * _bottomDistance, Color.green);

        _groundInfoTop = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y + _topDistance), Vector2.right * gameObject.transform.localScale.x, _topDistance, _layerMask);

        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y + _topDistance), Vector2.right * _topDistance * gameObject.transform.localScale.x, Color.green);

         if(_groundInfoBottom.collider == false || _groundInfoTop.collider == true)
         {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
         }
        return gameObject.transform.localScale.x;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        RaycastHit2D groundInfoCenter = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y + (_bottomDistance / 2)), Vector2.down, _bottomDistance, _layerMask);

        RaycastHit2D groundInfoTop = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x),
            gameObject.transform.position.y + (_topDistance / 2)), Vector2.down, _topDistance, _layerMask);

        if (groundInfoCenter.collider == false && groundInfoTop.collider == true)
        {
            _isJumping = true;
            return true;
        }
        
        return false;
    }
    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        return false;
    }
}
