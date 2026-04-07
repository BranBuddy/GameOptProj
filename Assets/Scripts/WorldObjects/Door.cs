using Unity.VisualScripting;
using UnityEngine;

public class Door : UnlockableTriggerInteraction
{
    [SerializeField] private Door doorToGoTo;

    public override void Interact(GameObject player)
    {
        base.Interact(player);
        GoThroughDoor(player);
    }

    private void GoThroughDoor(GameObject player)
    {
        if (doorToGoTo == null)
        {
            Debug.LogWarning("Door to go to is not assigned.");
            return;
        }

        Vector3 targetPosition = doorToGoTo.transform.position;
        player.transform.position = targetPosition;
    }


}
