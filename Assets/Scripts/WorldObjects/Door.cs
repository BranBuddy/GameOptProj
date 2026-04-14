/*
    Door that teleports the player to another door when interacted with. Plays SFX and supports linking to another door.
*/

using UnityEngine;

public class Door : UnlockableTriggerInteraction
{
    [SerializeField] private Door doorToGoTo;
    [SerializeField] private AudioClip _doorSFX;

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
        SoundManager.Instance.sfxSource.PlayOneShot(_doorSFX); // Play the door sound effect
        player.transform.position = targetPosition;
    }


}
