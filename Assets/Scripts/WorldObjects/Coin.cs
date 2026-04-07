using UnityEngine;

public class Coin : TriggerInteraction
{
    public int coinValue = 1;


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle inventory addition
        GameManager.Instance.IncreaseCoinCount(coinValue);
        StartCoroutine(DisableThenDestroy(this.gameObject));
    }

}
