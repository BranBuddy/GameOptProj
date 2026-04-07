using Unity.VisualScripting;
using UnityEngine;

public class TriggerInteraction : InteractionManager
{
    public string triggerID;
    public idType idType;
    public virtual void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true; // Ensure the collider is a trigger
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (idType == idType.Item)
            {
                if (!PlayerInventory.Instance.items.Contains(triggerID) && !string.IsNullOrEmpty(triggerID))
                {
                    PlayerInventory.Instance.items.Add(triggerID);
                }
            }
            else if (idType == idType.Checkpoint)
            {
                if (!PlayerInventory.Instance.checkpoints.Contains(triggerID) && !string.IsNullOrEmpty(triggerID))
                {
                    PlayerInventory.Instance.checkpoints.Add(triggerID);
                }
            }

            
        }
    }

}

public enum idType
{
    Item,
    Checkpoint
}