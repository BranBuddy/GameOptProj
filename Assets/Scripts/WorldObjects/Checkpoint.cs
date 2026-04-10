using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Checkpoint : TriggerInteraction
{
    private Vector3 _restartPosition;
    private bool collected = false;
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
            if (collected)
                return;

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
                    SoundManager.Instance.sfxSource.PlayOneShot(_triggerSFX); // Play the checkpoint activation sound effect
                    Debug.Log($"Checkpoint {triggerID} sprite changed.");
                    collected = true;
                }
            }
        }
    }
}
