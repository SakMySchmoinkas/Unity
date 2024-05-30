using UnityEngine;
using Cinemachine;

public class CameraLogic : MonoBehaviour
{
    public Transform playerTransform;
    public Transform camPos;
    public float smoothSpeed = 5f; // Adjust the speed of the smooth movement
    private CinemachineVirtualCamera virtualCamera;
    private bool isMovingToPlayer = false;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (playerTransform.CompareTag("Player"))
            {
                MoveCamera(camPos.position);

                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPosition = new Vector2(camPos.position.x, camPos.position.y);

                if (currentPosition == targetPosition && isMovingToPlayer)
                {
                    virtualCamera.Follow = camPos;
                    isMovingToPlayer = false;
                }
            }

            else if (playerTransform.CompareTag("Undetectable"))
            {
                MoveCamera(new Vector2(0f, 0f));
                virtualCamera.Follow = null;
                isMovingToPlayer = true;
            }
        }
    }

    void MoveCamera(Vector2 targetPosition)
    {
        // Gradually move towards the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}