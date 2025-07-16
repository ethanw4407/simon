using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Mono.Cecil.Cil;

public class GameManager : MonoBehaviour
{

    public static int distance_travelled;

    public static int render_distance = 10;

    Dictionary<string, Vector2> instructionList = new Dictionary<string, Vector2>() {

        { "HOLD UP", Vector2.up },
        { "HOLD RIGHT", Vector2.right },
        { "HOLD DOWN", Vector2.down },
        { "HOLD LEFT", Vector2.left },
        { "HOLD NOTHING", Vector2.zero }

    };

    string[] wordsLOL =
    {
        "",
        "3!",
        "2!",
        "1!",
        "GO!",
        "",
        "",
        "",
        "",
        ""
    };

    List<string> instructionListWords;

    Vector2 current_goal = Vector2.zero;

    public static GameManager instance;

    //simply here because you can't assign to static fields in the inspector and i'm too stupid to figure out a better solution
    //and also i'm a fan of using static fields + functions
    [SerializeField] Transform spp;
    [SerializeField] GameObject section;
    [SerializeField] Transform goal_direction;

    [SerializeField] TMP_Text instructions_text;

    [SerializeField] float course_correct = 10f;

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
        instance = this;
        instructionListWords = new List<string>(instructionList.Keys);
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
        if (turning.Equals(PlayerController.Lane.Middle)) {

            world.rotation = Goal_Direction.rotation;

            for (int i = 0; i < world.childCount; i++)
            {
                Transform sec = world.GetChild(i);
                sec.position += (-Vector3.forward) * current_speed * Time.deltaTime;

                sec.position = Vector3.MoveTowards(sec.position, new Vector3(Mathf.Round(sec.position.x / 10) * 10, sec.position.y, sec.position.z), Time.deltaTime * course_correct);
               
            }
        }

        else
        {
            int dir_int = 1 - (int) turning;
            ContinueTurn(dir_int);

            if (Mathf.Abs(world.transform.rotation.eulerAngles.y - goal_direction.rotation.eulerAngles.y) < 3f)
            {
                world.transform.rotation = Goal_Direction.rotation;
                turning = PlayerController.Lane.Middle;
            }
            
        }
    }

    public void StartTurn(PlayerController.Lane direction, Transform section_pos)
    {
        turning = direction;
        Goal_Direction.RotateAround(Vector3.zero, Vector3.up, (1 - (int)turning) * 90);

        if (direction != PlayerController.Lane.Middle)
        {

            spawn_platform_point.RotateAround(Vector3.zero, Vector3.down, (1 - (int)turning) * 90);
            spawn_platform_point.position = section_pos.position;
            spawn_platform_point.position += spawn_platform_point.forward * SectionSize;

            for (int i = 0; i < render_distance; i++)
            {
                GenerateNewSection();
            }

        }

    }

    void ContinueTurn(int dir_int)
    {

        world.RotateAround(Vector3.zero, transform.up, Mathf.Rad2Deg * dir_int * current_speed * Time.deltaTime / (SectionSize / 2f));

        for (int i = 0; i < world.childCount; i++)
        {
            Transform sec = world.GetChild(i);
            sec.position += (-Vector3.forward) * current_speed * Time.deltaTime;
        }

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


        if (distance_travelled % 10 == 0)
        {
            instance.Instruction();
        }
        else if (distance_travelled % 10 == 5)
        {
            instance.InstructionCheck();
        } else
        {
            instance.instructions_text.text = instance.wordsLOL[generated % 10];
        }

        spawn_platform_point.position += spawn_platform_point.forward * SectionSize;

    }

    public void Instruction()
    {

        string instr = instructionListWords[Random.Range(0, instructionListWords.Count)];

        instructions_text.text = instr;
        current_goal = instructionList[instr];

    }

    public void InstructionCheck()
    {
        if (PlayerController.instance.GetCurrentINPUT().Equals(current_goal))
        {
            Debug.Log("PASS");
        } else
        {
            Debug.Log("FAIL");
        }
    }


}
