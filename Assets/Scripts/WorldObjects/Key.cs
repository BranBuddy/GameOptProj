using UnityEngine;

public class Key : TriggerInteraction
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        SoundManager.Instance.sfxSource.PlayOneShot(_triggerSFX); // Play the key collection sound effect
        Debug.Log($"Key {triggerID} collected and added to inventory.");
        GameManager.Instance.StartCoroutine(DisableThenDestroy(this.gameObject));
    }
}
