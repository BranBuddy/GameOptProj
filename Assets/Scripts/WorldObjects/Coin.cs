using UnityEngine;

public class Coin : TriggerInteraction
{
    public int coinValue = 1;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        GameManager.Instance.IncreaseCoinCount(coinValue);
        SoundManager.Instance.sfxSource.PlayOneShot(_triggerSFX); // Play the coin collection sound effect
        GameManager.Instance.StartCoroutine(DisableThenDestroy(this.gameObject)); // Disable and then destroy the coin object
    }

}
