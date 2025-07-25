using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static int distance_travelled;

    public static int render_distance = 10;

    int current_instruction;

    public GameObject instructionCanvas;

    public int[] jklPressed = new int[3];
    string[] keys = { 
        "J", "K", "L"
    
    };





    Dictionary<string, Vector2> dirInstructionList = new Dictionary<string, Vector2>() {

        { "HOLD UP", Vector2.up },
        { "HOLD RIGHT", Vector2.right },
        { "HOLD DOWN", Vector2.down },
        { "HOLD LEFT", Vector2.left },
        { "HOLD NOTHING", Vector2.zero }

    };

    List<string> dirInstructionListWords;

    Dictionary<string, PlayerController.Lane> doorInstructionList = new Dictionary<string, PlayerController.Lane>()
    {
        { "LEFT LANE", PlayerController.Lane.Left },
        { "RIGHT LANE", PlayerController.Lane.Right },
        { "MIDDLE LANE", PlayerController.Lane.Middle }
    };

    List<string> doorInstructionListWords;

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

    

    Vector2 door_goal = Vector2.zero;
    int key_press_goal = -1;
    int key_press_amount = 0;

    PlayerController.Lane goal_door;
    PlayerController.Lane goal_turn;



    public static GameManager instance;

    //simply here because you can't assign to static fields in the inspector and i'm too stupid to figure out a better solution
    //and also i'm a fan of using static fields + functions

    //adding this comment later i forgot about the static instance strategy somehow???
    //this code is getting worse day by day
    [SerializeField] Transform spp;
    [SerializeField] GameObject section;
    [SerializeField] Transform goal_direction;

    [SerializeField] Transform spawn_special;
    [SerializeField] GameObject turn_platform;



    [SerializeField] TMP_Text instructions_text;

    [SerializeField] float course_correct = 10f;

    static Transform world;
    [SerializeField] Transform current_direction;
    public static Transform spawn_platform_point;

    public static GameObject Sect;
    public const int SectionSize = 10;
    public static int generated = 0;

    static Transform Goal_Direction;

    public static float current_speed = 30f;

    public static float rotate_speed = 1f;

    static PlayerController.Lane turning = PlayerController.Lane.Middle;
    static bool is_turning = false;

    public static bool LYING = false;

    [SerializeField] Color blue;
    [SerializeField] Color orange;

    [SerializeField] TMP_Text distance_text;
    [SerializeField] GameObject death_SCREEN;

    private void Awake()
    {
        instance = this;
        dirInstructionListWords = new List<string>(dirInstructionList.Keys);
        doorInstructionListWords = new List<string>(doorInstructionList.Keys);
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

        
        if (!is_turning) {
            turning = PlayerController.instance.GetCurrentINPUT().x == 0 ? PlayerController.Lane.Middle : (PlayerController.instance.GetCurrentINPUT().x < 0 ? PlayerController.Lane.Left : PlayerController.Lane.Right);
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
                is_turning = false;
            }
            
        }
    }

    public void StartTurn(PlayerController.Lane direction, Transform section_pos)
    {
        turning = direction;
        is_turning = true;
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

    public void TraversedSection(Transform section) {

        distance_travelled++;

        distance_text.text = "Distance Travelled: " + distance_travelled;

        spawn_special.position = section.position + (section.forward * SectionSize * 5);
        spawn_special.rotation = spawn_platform_point.rotation;



        GenerateNewSection();

        if (distance_travelled < 8f)
            return;

        if (distance_travelled % 10 == 0)
        {
            instance.Instruction();

            current_speed+=0.5f;


            instructionCanvas.SetActive(true);
        }
        else if (distance_travelled % 10 == 5)
        {
            instance.InstructionCheck();
        }
        else if (distance_travelled < 5)
        {
            instance.instructions_text.text = instance.wordsLOL[generated % 10];
        }
        else
        {

        }
    }

    static void GenerateNewSection()
    {
        generated++;

        var new_section = Instantiate(Sect, spawn_platform_point);
        new_section.transform.parent = world;

        spawn_platform_point.position += spawn_platform_point.forward * SectionSize;
    }

    public void Instruction()
    {
        //STRUGGLE CODE ATP TS IS KILLING ME LOL
        //UGLIEST CODE IN THE WEST
        int instrType = Random.Range(0, 4);

        float isLying = Random.value;

        LYING = isLying <= 0.25f;

        instructions_text.color = LYING ? blue : orange;



        current_instruction = instrType;


        string instr = "";

        switch (instrType) {
            case 0: //HOLD A DIRECTION

                instr = dirInstructionListWords[Random.Range(0, dirInstructionListWords.Count)];
                
                door_goal = dirInstructionList[instr];

                break;
            case 1: //DOORS

                int doorNum = Random.Range(0, doorInstructionListWords.Count);      

                if (doorInstructionList[doorInstructionListWords[Random.Range(0, doorInstructionListWords.Count)]] == PlayerController.instance.current_lane) {
                    doorNum += 1;
                    doorNum %= 3;

                }

                instr = doorInstructionListWords[doorNum];

                goal_door = doorInstructionList[instr];

                //just generate the doors now 

                break;

            case 2: //key n times

                int amount = Random.Range(2, 6);
                int key = Random.Range(0, 3);

                instr = "Press " + keys[key] + " " + amount + " times";
                key_press_goal = key;
                key_press_amount = amount;

                break;

            case 3: //turn
                
                int direction_to_turn = Random.Range(0, 3);

                switch (direction_to_turn) { 
                    case 0:
                        instr = "Turn LEFT";
                        goal_turn = PlayerController.Lane.Left;
                        break;
                    case 1:
                        instr = "Keep going STRAIGHT";
                        goal_turn = PlayerController.Lane.Middle;
                        break;
                    case 2:
                        instr = "Turn RIGHT";
                        goal_turn = PlayerController.Lane.Right;
                        break;
                }


                //generate the turn

                Transform new_turn = Instantiate(turn_platform, spawn_special).transform;

                new_turn.SetParent(world.transform);
                new_turn.rotation = Quaternion.identity;


                break;
        }

        instructions_text.text = instr;

    }

    public void InstructionCheck()
    {

        bool condition;

        switch (current_instruction)
        {
            case 0:

                condition = PlayerController.instance.GetCurrentINPUT().Equals(door_goal);

                condition = LYING ? !condition : condition;

                if (condition)
                {
                    Pass();
                }
                else
                {
                    Fail();
                }

                break;

            case 1:

                condition = PlayerController.instance.current_lane == goal_door;
                condition = LYING ? !condition : condition;

                if (condition)
                {
                    Pass();
                } else
                {
                    Fail();
                }

                    break;

            case 2:

                condition = jklPressed[key_press_goal] == key_press_amount && jklPressed[(key_press_goal + 1) % 3] == 0 && jklPressed[(key_press_goal + 2) % 3] == 0;
                condition = LYING ? !condition : condition;


                if (condition)
                {
                    Pass();
                    break;
                }
                Fail();

                break;

            case 3:
                condition = turning == goal_turn;
                condition = LYING ? !condition : condition;

                if (condition)
                {
                    Pass();
                    Debug.Log("PASSED");
                }
                else
                {
                    Fail();
                }
                break;

        }

        jklPressed = new int[3];

    }

    void Pass()
    {
        instructions_text.text = "Well Done!";
    }

    void Fail()
    {
        instructions_text.text = "Nope!";
        current_speed = 0f;
        death_SCREEN.SetActive(true);
    }


}
