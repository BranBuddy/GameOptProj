using UnityEditor.ShaderGraph;
using UnityEngine;
using System.Collections.Generic;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public float friction { get; private set; }
    public WallType wallType { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    public List<Vector2> ContactPoints { get; private set; } = new List<Vector2>();
    private PhysicsMaterial2D _material;
    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluteCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluteCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        onWall = false;
        friction = 0;
    }

    public void EvaluteCollision(Collision2D collision)
    {
        ContactPoints.Clear();
        for(int i=0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            ContactPoints.Add(collision.GetContact(i).point); // Add contact point
            onGround |= ContactNormal.y >= .9f;
            onWall |= Mathf.Abs(ContactNormal.x) >= .9f; // Consider it a wall if the normal is mostly horizontal
        }

        if(onWall)
            wallType = CheckWhatWallType(collision);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            onGround = false;
            if(CheckIfPlayerIsOnSlopeTip(collision))
            {
                if(IsOnLeftOfSlope(collision))
                    _body.AddForce(Vector2.left * _body.mass * Physics2D.gravity.magnitude * .5f, ForceMode2D.Force);
                else
                    _body.AddForce(Vector2.right * _body.mass * Physics2D.gravity.magnitude * .5f, ForceMode2D.Force);
            }
        }
    }

    private WallType CheckWhatWallType(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("StickyWall"))
        {
            // Implement sticky wall behavior
            return WallType.Sticky;
        }
        else if(collision.gameObject.CompareTag("BouncyWall"))
        {
            // Implement bouncy wall behavior
            return WallType.Bouncy;
        }
        else if(collision.gameObject.CompareTag("UnjumpableWall"))
        {
            // Implement unjumpable wall behavior
            return WallType.Unjumpable;
        }
        return WallType.None;
    }

    private bool CheckIfPlayerIsOnSlopeTip(Collision2D collision)
    {
        for(int i=0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            if(ContactNormal.y >= .9f)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOnLeftOfSlope(Collision2D collision)
    {
        for(int i=0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            if(ContactNormal.x < 0f)
            {
                return true;
            }
        }
        return false;
    }

    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody.sharedMaterial;

        friction = 0;

        if(_material != null)
        {
            friction = _material.friction;
        }
    }

    public bool GetOnGround() => onGround;
    public float GetFriction() => friction;

}

public enum WallType
{
    None,
    Sticky,
    Bouncy,
    Unjumpable
}