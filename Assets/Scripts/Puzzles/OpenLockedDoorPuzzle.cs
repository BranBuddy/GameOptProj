/*
    This script is used to progress through the game if the player has the correct key in their inventory when they collide with the locked door.
*/

using System.Collections.Generic;
using UnityEngine;

public class OpenLockedDoorPuzzle : CollisionInteraction
{
    public GameObject lockedDoor;
    public Key key;
    private string requiredKeyID;
    [SerializeField] private AudioClip _lockOpenSFX;

    public List<GameObject> lockWalls;

    private void Start()
    {
        if (key != null)
        {
            requiredKeyID = key.GetComponent<Key>().triggerID;
        }
        else
        {
            Debug.LogError("Key reference is missing in OpenLockedDoorPuzzle.");
        }
    } 

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); // Call the base method to handle any additional logic
        Debug.Log($"[OpenLockedDoorPuzzle] Collided with: {collision.gameObject.name}, normal: {collision.contacts[0].normal}");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerInventory.Instance.items.Contains(requiredKeyID))
            {
                lockedDoor.SetActive(false); // Open the door by deactivating it

                foreach (GameObject wall in lockWalls)
                {
                    wall.SetActive(false); // Deactivate the lock walls
                }
                
                PlayerInventory.Instance.items.Remove(requiredKeyID); // Remove the key from inventory
                SoundManager.Instance.sfxSource.PlayOneShot(_lockOpenSFX); // Play the lock opening sound effect
            }
            else
            {
                Debug.Log("You need the correct key to open this door.");
            }
        }
    } 



}
