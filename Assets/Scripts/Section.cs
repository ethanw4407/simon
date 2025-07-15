using System.Collections;
using UnityEngine;
using static PlayerController;

public class Section : MonoBehaviour
{

    public
    enum type { 
        basic,
        turn, 
        new_instruction
    }

    public type section_type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player_check = other.transform.parent.GetComponent<PlayerController>();
        Debug.Log(other.name);
        

        if (player_check != null)
        {

            GameManager.TraversedSection();

            switch (section_type) {
                case type.basic:
                    break;
                case type.turn:
                    player_check.StartTurn();
                    break;
                case type.new_instruction:
                    break;

            }
            
        }
    }
}
