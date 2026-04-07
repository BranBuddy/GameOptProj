    
using UnityEngine;
using UnityEngine.InputSystem;
public class Interact : MonoBehaviour
{
    [SerializeField] private InputActionReference interactID;
    [SerializeField] private UnlockableTriggerInteraction currentInteraction = null;
    [SerializeField] private bool canInteract = false;
    [SerializeField] private string whatIsBeingInteractedWith = null; // Unique identifier for this

    private void OnEnable()
    {
        if (interactID == null)
        {
            Debug.LogError("InteractID is not assigned in the inspector. Please assign an InputActionReference for interaction.");
            return;
        }
        Debug.Log("Interact.OnEnable: Subscribing to input action and enabling action");
        interactID.action.performed += InteractWithObject;
        interactID.action.Enable();
    }

    private void OnDisable()
    {
        if (interactID == null)
        {
            Debug.LogError("InteractID is not assigned in the inspector. Please assign an InputActionReference for interaction.");
            return;
        }
        Debug.Log("Interact.OnDisable: Unsubscribing from input action");
        interactID.action.performed -= InteractWithObject;
        interactID.action.Disable();
    }

     private void InteractWithObject(InputAction.CallbackContext context)
    {
        Debug.Log($"InteractWithObject called. canInteract={canInteract}, currentInteraction={currentInteraction}, whatIsBeingInteractedWith={whatIsBeingInteractedWith}");
        if (canInteract && currentInteraction != null)
        {
            Debug.Log($"Interacted with {whatIsBeingInteractedWith}");
            currentInteraction.Interact(this.gameObject); // Call the Interact method on the current interaction
            canInteract = false; // Prevent multiple interactions until the player exits and re-enters the trigger
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<UnlockableTriggerInteraction>() != null && this.gameObject == GameManager.Instance.currentPlayer) // Check if the collided object has an UnlockableTriggerInteraction and if this player is active
        {
            currentInteraction = collision.gameObject.GetComponent<UnlockableTriggerInteraction>();
            whatIsBeingInteractedWith = currentInteraction.name; // Set the name of the object being interacted with
            // Implement specific interaction logic here, such as unlocking a door or triggering an event
            Debug.Log($"Player triggered {this.gameObject.name}");
            InteractionUIManager.Instance.ShowInteractionText(whatIsBeingInteractedWith); // Show interaction text with the name of the object
            canInteract = true;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<UnlockableTriggerInteraction>() != null)
        {
            currentInteraction = null;

            if (InteractionUIManager.Instance != null)
            {
                InteractionUIManager.Instance.HideInteractionText(); // Clear interaction text
            }
            else
            {
                Debug.LogWarning("InteractionUIManager.Instance is null in OnTriggerExit2D. Please ensure it exists in the scene and is properly assigned.");
            }
            canInteract = false;
        }
    }
}
