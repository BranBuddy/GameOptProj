/*
    Base class for obstacles that reset the player on collision (e.g., spikes, lava). Plays a death SFX and triggers player reset logic.
*/

using UnityEngine;

public class ResetObstacle : MonoBehaviour
{
    [SerializeField] private AudioClip deathSFX;
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Restart>().Reset();
            SoundManager.Instance.sfxSource.PlayOneShot(deathSFX);
        }
    }
}
