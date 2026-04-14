/*
    Handles coin collection logic, including incrementing the coin count, playing SFX, and destroying the coin object after collection.
*/

using UnityEngine;

public class Coin : TriggerInteraction
{
    public int coinValue;
    private bool _collected = false;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (_collected) return;
        _collected = true;
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        GameManager.Instance.IncreaseCoinCount(coinValue);
        SoundManager.Instance.sfxSource.PlayOneShot(_triggerSFX); // Play the coin collection sound effect
        GameManager.Instance.StartCoroutine(DisableThenDestroy(this.gameObject)); // Disable and then destroy the coin object
    }
}
