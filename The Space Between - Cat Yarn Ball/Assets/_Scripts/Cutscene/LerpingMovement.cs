using UnityEngine;

public class LerpingMovement : MonoBehaviour
{
    public Transform targetTransform;  // The target transform to which you want to lerp
    public float lerpSpeed = 2.0f;     // The speed of the lerp

    private void Update()
    {
        if (targetTransform != null)
        {
            // Calculate the new position to move towards using Lerp
            Vector3 newPosition = Vector3.Lerp(transform.position, targetTransform.position, lerpSpeed * Time.deltaTime);

            // Move the current transform towards the target position
            transform.position = newPosition;
        }
    }

    public void SetNewTarget(Transform transform){
        targetTransform = transform;
    }
}
