using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Lane { 
        Left,
        Middle,
        Right,
    }

    Vector2 WASD;
    Vector3 goal_position = Vector2.zero;
    float current_speed;

    private Lane current_lane = Lane.Middle;
    [SerializeField] float lane_change_speed;

    public static Transform player_transform;

    Rigidbody rb;
    PlayerInput input;


    [SerializeField] Transform player;
    public static Transform Player;
    [SerializeField] Transform left_pos;
    [SerializeField] Transform right_pos;
    [SerializeField] Transform mid_pos;

    private void Awake()
    {
        Player = player;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        player_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (current_lane == Lane.Left)
        {
            goal_position = left_pos.position;
        }
        else if (current_lane == Lane.Right)
        {
            goal_position = right_pos.position;
        }
        else
        {
            goal_position = mid_pos.position;
        }

        player.position = Vector3.MoveTowards(player.position, goal_position, lane_change_speed * Time.deltaTime);

    }

    public void StartTurn()
    {
        if (WASD.x < 0)
            GameManager.StartTurn(Lane.Left);
        else if (WASD.x > 0)
            GameManager.StartTurn(Lane.Right);
        else
            GameManager.StartTurn(Lane.Middle);
    }

    

    private void OnMove(InputValue inputValue)
    {
        WASD = inputValue.Get<Vector2>();
    }

    private void OnMoveLeft()
    {
        current_lane = (Lane)Mathf.Clamp((int)(current_lane - 1), 0, 2);
    }

    private void OnMoveRight()
    {
        current_lane = (Lane)Mathf.Clamp((int)(current_lane + 1), 0, 2);
    }
}
