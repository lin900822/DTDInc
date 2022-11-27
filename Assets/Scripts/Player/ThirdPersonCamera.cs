using System.Collections;
using UnityEngine;
using Fusion;

public class ThirdPersonCamera : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraTrans = null;
    [SerializeField] private Vector2 cameraDistanceMinMax = new Vector2(.5f, 5f);
    [SerializeField] private LayerMask collisionLayer = default;

    private Vector3 cameraDirection;
    private float cameraDistance;

    public override void Spawned()
    {
        cameraDirection = playerCameraTrans.localPosition.normalized;
        cameraDistance = cameraDistanceMinMax.y;
    }

    public void OnRender()
    {
        CheckCameraOcclusionAndCollision(playerCameraTrans);
    }

    private void CheckCameraOcclusionAndCollision(Transform cameraTrans)
    {
        Vector3 desiredCameraPosition = transform.TransformPoint(cameraDirection * cameraDistanceMinMax.y);
        RaycastHit hit;
        if (Physics.Linecast(transform.position, desiredCameraPosition, out hit, collisionLayer))
        {
            cameraDistance = Mathf.Clamp(hit.distance, cameraDistanceMinMax.x, cameraDistanceMinMax.y);
        }
        else
        {
            cameraDistance = cameraDistanceMinMax.y;
        }

        cameraTrans.localPosition = cameraDirection * cameraDistance;
    }
}