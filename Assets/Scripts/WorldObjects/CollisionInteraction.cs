using UnityEngine;

public class CollisionInteraction : InteractionManager
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // This method can be overridden by derived classes to handle collision interactions
    }
}
