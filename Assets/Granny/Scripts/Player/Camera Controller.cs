using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float cameraHeight = 1.6f;
    public float smoothSpeed = 10f;
    public LayerMask collisionMask;
    public float cameraRadius = 0.3f;
    private Vector3 desiredPosition;

    void LateUpdate()
    {
        Vector3 TargetPosition = player.position + player.forward * cameraRadius + Vector3.up * cameraHeight;
        desiredPosition = TargetPosition;

        RaycastHit hit;
        if ( Physics.SphereCast(player.position + Vector3.up * cameraHeight, cameraRadius, (desiredPosition - (player.position + Vector3.up * cameraHeight)).normalized, out hit, cameraRadius, collisionMask))
        {
            desiredPosition = hit.point + hit.normal * cameraRadius;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
