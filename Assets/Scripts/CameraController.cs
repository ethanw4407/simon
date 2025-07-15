using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;

    Vector3 goal_position;
    private void Update()
    {
        goal_position = player.position + camOffset;
    }



}