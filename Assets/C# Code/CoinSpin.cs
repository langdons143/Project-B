// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=ES2jPGnWQoU

using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 100f;

    [Header("Floating")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // Makes the coin spin
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Makes the coin float up and down
        float newY = startPosition.y +
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.localPosition = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}