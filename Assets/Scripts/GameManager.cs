using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static int distance_travelled;

    public static int render_distance = 10;

    //simply here because you can't assign to static fields in the inspector and i'm too stupid to figure out a better solution
    //and also i'm a fan of using static fields + functions
    [SerializeField] Transform spp;
    [SerializeField] GameObject section;
    [SerializeField] Transform goal_direction;

    static Transform world;
    [SerializeField] Transform current_direction;
    public static Transform spawn_platform_point;

    public static GameObject Sect;
    public const int SectionSize = 10;
    public static int generated = 0;

    static Transform Goal_Direction;

    public static float current_speed = 10f;

    public static float rotate_speed = 0.5f;

    static PlayerController.Lane turning = PlayerController.Lane.Middle;

    private void Awake()
    {
        world = transform.GetChild(0);
        spawn_platform_point = spp;
        Sect = section;
        Goal_Direction = goal_direction;
    }

    private void Start()
    {
        //first generation
        for (int i = 0; i < render_distance; i++)
        {
            var new_section = Instantiate(Sect, spawn_platform_point);
            new_section.transform.parent = world;

            spawn_platform_point.position += spawn_platform_point.forward * SectionSize;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (turning.Equals(PlayerController.Lane.Middle))
        {
            for (int i = 0; i < world.childCount; i++)
            {
                world.GetChild(i).position += (-current_direction.forward) * current_speed * Time.deltaTime;
            }
        }
        else
        {
            int dir_int = 1 - (int) turning;
            ContinueTurn(dir_int);

            if (Mathf.Abs(world.transform.rotation.eulerAngles.z - goal_direction.rotation.eulerAngles.z) < 0.1)
            {
                world.transform.rotation = Goal_Direction.rotation;
            }

            
            
            turning = PlayerController.Lane.Middle;
        }
    }

    public static void StartTurn(PlayerController.Lane direction)
    {
        turning = direction;
        Goal_Direction.RotateAround(Vector3.zero, Vector3.up, 1 - (int)turning * 90);
        spawn_platform_point.RotateAround(Vector3.zero, Vector3.up, 1 - (int)turning * 90);
        spawn_platform_point.position += spawn_platform_point.forward * SectionSize;

        for (int i = 0; i < render_distance; i++)
        {
            GenerateNewSection();
        }
        Debug.Log("turned" + direction.ToString());

    }

    public static void TraversedSection() {

        distance_travelled++;

        GenerateNewSection();
    }

    static void GenerateNewSection()
    {
        generated++;

        var new_section = Instantiate(Sect, spawn_platform_point);
        new_section.transform.parent = world;

        if (Random.value < 0.15)
        {
            new_section.GetComponent<Section>().section_type = Section.type.turn;
        }

        spawn_platform_point.position += spawn_platform_point.forward * SectionSize;

    }

    void ContinueTurn(int dir_int) {

        float r = 5f - Mathf.Abs(PlayerController.Player.position.x);

        world.RotateAround(Vector3.zero, transform.up, Mathf.Rad2Deg * dir_int * current_speed * Time.deltaTime / r);

    }
}
