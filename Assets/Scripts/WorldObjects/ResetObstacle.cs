using UnityEngine;

public class ResetObstacle : MonoBehaviour
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Restart>().Reset();
        }
    }
}
