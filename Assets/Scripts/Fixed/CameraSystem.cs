using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public float cameraSpeed = 6f;

    private Vector3 originalPosition = Vector3.zero;
    private bool canMoveLeft = false;
    private bool canMoveRight = false;

    public void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    private void LateUpdate()
    {
        if (canMoveLeft == true)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - Time.deltaTime * cameraSpeed);
        }

        if (canMoveRight == true)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + Time.deltaTime * cameraSpeed);
        }
    }

    public void StopToMove()
    {
        if (canMoveLeft == true)
        {
            canMoveLeft = false;
        }

        if (canMoveRight == true)
        {
            canMoveRight = false;
        }
    }

    public void MoveToLeft()
    {
        canMoveLeft = true;
    }

    public void MoveToRight()
    {
        canMoveRight = true;
    }

    public void ResetCamera()
    {
        gameObject.transform.position = originalPosition;
    }
}
