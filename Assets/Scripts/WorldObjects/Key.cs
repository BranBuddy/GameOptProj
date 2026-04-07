using UnityEngine;

public class Key : TriggerInteraction
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        StartCoroutine(DisableThenDestroy(this.gameObject));
    }
}
