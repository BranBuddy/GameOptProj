using System;
using System.Collections;
using UnityEngine;
public class Checkpoint : TriggerInteraction
{
    private Vector3 _restartPosition;
    [SerializeField] private Sprite spriteToChangeTo;
    public override void Start()
    {
        base.Start();
        _restartPosition = this.transform.position;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        if (collision.CompareTag("Player"))
        {
            Restart restartComponent = collision.GetComponent<Restart>();
            if (restartComponent != null)
            {
                restartComponent._restartPosition = _restartPosition;
            }

            Debug.Log($"Checkpoint {triggerID} collected. Restart position updated to {_restartPosition}.");

            if (spriteToChangeTo != null)
            {
                SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = spriteToChangeTo;
                    Debug.Log($"Checkpoint {triggerID} sprite changed.");
                }
            }
        }
    }
}
