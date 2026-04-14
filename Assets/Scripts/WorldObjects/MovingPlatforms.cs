using System.Collections;
using UnityEngine;

[System.Serializable]
public class DirectionalPlatformData
{
    public bool moveUp;
    public bool moveDown;
    public bool moveLeft;
    public bool moveRight;
    public float speed;
    public float distance;
}

public class MovingPlatforms : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    public DirectionalPlatformData platformData;

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition;

        if (platformData.moveUp)
            _endPosition += Vector3.up * platformData.distance;
        if (platformData.moveDown)
            _endPosition += Vector3.down * platformData.distance;
        if (platformData.moveLeft)
            _endPosition += Vector3.left * platformData.distance;
        if (platformData.moveRight)
            _endPosition += Vector3.right * platformData.distance;

        _endPosition = _startPosition + (_endPosition - _startPosition).normalized * platformData.distance; // Ensure consistent movement distance

        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            // Move towards the end position
            while (Vector3.Distance(transform.position, _endPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _endPosition, platformData.speed * Time.deltaTime);
                yield return null;
            }

            // Swap start and end positions
            Vector3 temp = _startPosition;
            _startPosition = _endPosition;
            _endPosition = temp;
        }
    
    }}
