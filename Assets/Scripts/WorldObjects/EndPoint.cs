/*
    Represents the level end point. Checks if the required player reaches the end and triggers level completion logic.
*/

using UnityEngine;

public class EndPoint : CollisionInteraction
{
    [SerializeField] private GameObject playerRequired;
    [SerializeField] private EndPoint otherEndPoint;
    public bool readyToFinish;

    private void Start()
    {
        readyToFinish = false;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == playerRequired)
        {
            readyToFinish = true;
            if (otherEndPoint != null && otherEndPoint.readyToFinish)
            {
                GameManager.Instance.CompleteGame(); // Reward the player with coins for finishing the level
                Debug.Log("Level Completed!"); // You can replace this with your level completion logic
            }
        }
    }

    
}
