using UnityEngine;

public class OpenLockedDoorPuzzle : CollisionInteraction
{
    public GameObject lockedDoor;
    public Key key;
    private string requiredKeyID;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerInventory.Instance.items.Contains(requiredKeyID))
            {
                lockedDoor.SetActive(false); // Open the door by deactivating it
                PlayerInventory.Instance.items.Remove(requiredKeyID); // Remove the key from inventory
            }
            else
            {
                Debug.Log("You need the correct key to open this door.");
            }
        }
    } 



}
