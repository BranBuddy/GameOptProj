using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ChainRope : CollisionInteraction
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Static; // Start as static so it doesn't fall immediately
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    private IEnumerator DropChain()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before dropping the chain
        rb.bodyType = RigidbodyType2D.Dynamic; // Make the chain affected by physics
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // Freeze horizontal movement and rotation
        isFalling = true; // Set the flag to indicate the chain is falling
    }


    private IEnumerator ShrinkChainAfterDrop()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before starting to shrink the chain
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(startScale.x, 0, startScale.z); // Target scale with zero height
        float shrinkDuration = 1f; // Duration of the shrinking effect
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Ensure the chain is fully shrunk at the end
        if (col != null) col.enabled = false; // Disable collider when fully shrunk
        rb.bodyType = RigidbodyType2D.Static; // Make the chain static again after shrinking
        isFalling = false; // Reset the falling flag after shrinking
        StartCoroutine(RegenerateChain()); // Start regenerating the chain after shrinking
    }

    private IEnumerator GrowChain()
    {
        // Set scale to zero height at the start of the coroutine
        transform.localScale = new Vector3(originalScale.x, 0, originalScale.z);
        if (col != null) col.enabled = true; // Enable collider before growing
        Vector3 startScale = transform.localScale;
        float growDuration = 1f; // Duration of the growing effect
        float elapsedTime = 0f;

        while (elapsedTime < growDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, originalScale, elapsedTime / growDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // Ensure the chain is fully grown at the end
    }

    private IEnumerator DropAndShrinkChain()
    {
        yield return DropChain(); // Wait for the chain to drop
        yield return ShrinkChainAfterDrop(); // Then start shrinking the chain
    }

    private IEnumerator RegenerateChain()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before regenerating the chain
        ResetObject();
        yield return GrowChain();
    }

    private void ResetObject()
    {
        transform.position = originalPosition; // Reset position
        rb.bodyType = RigidbodyType2D.Static; // Make the chain static again
        isFalling = false; // Allow chain to fall again
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(DropAndShrinkChain()); // Start the process of dropping and shrinking the chain
        }
    }
}
