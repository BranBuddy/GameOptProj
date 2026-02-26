using UnityEditor.ShaderGraph;
using UnityEngine;
using System.Collections.Generic;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public float friction { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    public List<Vector2> ContactPoints { get; private set; } = new List<Vector2>();
    private PhysicsMaterial2D _material;

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
            onWall |= Mathf.Abs(ContactNormal.x) >= .9f;
        }
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
