using UnityEngine;
using System.Collections.Generic;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public float friction { get; private set; }
    public WallType wallType { get; private set; }
    public FloorType floorType { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    public List<Vector2> ContactPoints { get; private set; } = new List<Vector2>();
    private PhysicsMaterial2D _material;
    private Rigidbody2D _body;

    public List<Collision2D> _wallCollisions = new List<Collision2D>();

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluteCollision(collision);
        RetrieveFriction(collision);

        if (onWall)
            _wallCollisions.Add(collision);
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
        wallType = WallType.None;
        floorType = FloorType.None;
        ContactNormal = Vector2.zero;
        ContactPoints.Clear();
        _wallCollisions.Clear();
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

        if(onGround)
            floorType = CheckWhatFloorType(collision);

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

    private FloorType CheckWhatFloorType(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("StickyFloor"))
        {
            // Implement sticky floor behavior
            return FloorType.Sticky;
        }
        else if(collision.gameObject.CompareTag("BouncyFloor"))
        {
            // Implement bouncy floor behavior
            return FloorType.Bouncy;
        }
        else if(collision.gameObject.CompareTag("IcyFloor"))
        {
            // Implement icy floor behavior
            return FloorType.Icy;
        }
        return FloorType.None;
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
        else if(collision.gameObject.CompareTag("Wall"))
        {
            return WallType.None; // Regular wall with no special properties
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
        _material = collision.rigidbody != null 
            ? collision.rigidbody.sharedMaterial 
            : collision.collider.sharedMaterial;

        friction = 0;

        if(_material != null)
        {
            friction = _material.friction;
        }
    }

    public bool GetOnGround() => onGround;
    public float GetFriction() => friction;

}

public enum FloorType
{
    None,
    Sticky,
    Bouncy,
    Icy
}

public enum WallType
{
    None,
    Sticky,
    Bouncy,
    Unjumpable
}