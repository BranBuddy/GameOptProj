using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : CollisionInteraction
{
    private Rigidbody2D rb;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isFalling = false;
    private bool isShrinking = false;
    private bool isGrowing = false;

    [SerializeField] private AudioClip _fallingSFX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; // Start as static so it doesn't fall immediately
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    private IEnumerator FallAfterDelay(Collider2D collision)
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before falling
        StartCoroutine(JutterPlatformBeforeFall()); // Start the juttering effect before falling
        yield return new WaitForSeconds(0.5f); // Wait for the juttering
        SoundManager.Instance.sfxSource.PlayOneShot(_fallingSFX); // Play the falling sound effect
        StartCoroutine(DropPlatformToCertainDistance()); // Start dropping the platform
        StartCoroutine(ShrinkPlatformAfterFall()); // Start the shrinking effect after falling
    }

    private IEnumerator DropPlatformToCertainDistance()
    {
        float dropDistance = 5f; // Distance to drop
        float dropSpeed = 2f; // Speed of dropping
        Vector3 targetPosition = originalPosition - new Vector3(0, dropDistance, 0); // Target position after dropping
        rb.bodyType = RigidbodyType2D.Dynamic; // Make the platform dynamic so it can fall

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dropSpeed * Time.deltaTime);
            yield return null;
        }
        rb.bodyType = RigidbodyType2D.Static; // Make the platform static again after reaching the target position 
    }

    private IEnumerator JutterPlatformBeforeFall()
    {
        float jutterDuration = 0.5f; // Duration of the juttering effect
        float elapsedTime = 0f;

        while (elapsedTime < jutterDuration)
        {
            float jutterAmount = Mathf.Sin(elapsedTime * 20f) * 0.1f; // Jutter effect using sine wave
            transform.position = originalPosition + new Vector3(jutterAmount, 0, 0); // Jutter horizontally
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset to original position after juttering
    }

    private IEnumerator ShrinkPlatformAfterFall()
    {

        float shrinkDuration = .5f; // Duration of the shrinking effect
        float elapsedTime = 0f;
        isShrinking = true;

        while (elapsedTime < shrinkDuration)
        {
            float shrinkAmount = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration); // Lerp from original scale to zero
            transform.localScale = originalScale * shrinkAmount; // Shrink the platform
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Wait for 1 second before resetting the platform
        isShrinking = false;
        ResetObject(); // Reset the platform to its original position and scale
        StartCoroutine(GrowPlatformAfterReset()); // Start the growing effect after resetting

    }

    private IEnumerator GrowPlatformAfterReset()
    {
        float growDuration = 1f; // Duration of the growing effect
        float elapsedTime = 0f;
        Vector3 currentScale = transform.localScale; // Get the current scale (which should be zero after shrinking)
        isGrowing = true;
        while (elapsedTime < growDuration)
        {
            float growAmount = Mathf.Lerp(0f, 1f, elapsedTime / growDuration); // Lerp from zero to original scale
            transform.localScale = currentScale * growAmount; // Grow the platform
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isGrowing = false;
        transform.localScale = originalScale; // Ensure the platform is at its original scale after growing
    }

    private void ResetObject()
    {
        StopAllCoroutines(); // Stop any ongoing coroutines that might affect position
        rb.bodyType = RigidbodyType2D.Static; // Ensure platform is not affected by physics
        rb.linearVelocity = Vector2.zero; // Stop any movement
        rb.angularVelocity = 0f;
        transform.position = originalPosition; // Reset position
        transform.localScale = originalScale; // Reset scale
        isFalling = false; // Allow platform to fall again
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); // Call the base method to handle player reset
        if (collision.collider.CompareTag("Player"))
        {
            if (!isFalling && !isShrinking && !isGrowing)
            {
                isFalling = true;
                StartCoroutine(FallAfterDelay(collision.collider)); // Start the falling process after a delay
            }
        }
    }
}
