using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlockableTriggerInteraction : InteractionManager
{
    [SerializeField] private string whatIsBeingTriggered; // Unique identifier for this trigger

    public virtual void Interact(GameObject player)
    {
        Debug.Log($"Interacted with {whatIsBeingTriggered}");
        // Implement specific interaction logic here, such as unlocking a door or triggering an event
    }
    
}
