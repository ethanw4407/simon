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

    public static PlayerController instance;

    public Lane current_lane = Lane.Middle;
    [SerializeField] float lane_change_speed;

    public static Transform player_transform;

    PlayerInput input;


    [SerializeField] Transform player;
    public static Transform Player;
    [SerializeField] Transform left_pos;
    [SerializeField] Transform right_pos;
    [SerializeField] Transform mid_pos;

    private void Awake()
    {
        instance = this;
        Player = player;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public void StartTurn(Transform section)
    {
        if (WASD.x < 0)
            GameManager.instance.StartTurn(Lane.Left, section);
        else if (WASD.x > 0)
            GameManager.instance.StartTurn(Lane.Right, section);
        else
            GameManager.instance.StartTurn(Lane.Middle, section);
    }

    public Vector2 GetCurrentINPUT()
    {
        return WASD;

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

    private void OnJ()
    {
        GameManager.instance.jklPressed[0]++;
    }

    private void OnK()
    {
        GameManager.instance.jklPressed[1]++;
    }

    private void OnL()
    {
        GameManager.instance.jklPressed[2]++;
    }
}
