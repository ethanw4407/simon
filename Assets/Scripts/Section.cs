using System.Collections;
using UnityEngine;
using static PlayerController;

public class Section : MonoBehaviour
{

    [SerializeField] float destroy_behind = 10f;

    public
    enum type { 
        basic,
        turn, 
        new_instruction
    }

    public type section_type;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController player_check = other.transform.parent.GetComponent<PlayerController>();

        if (player_check != null)
        {

            GameManager.TraversedSection();

            switch (section_type) {
                case type.basic:
                    break;
                case type.turn:
                    player_check.StartTurn(transform);
                    break;
                case type.new_instruction:
                    break;

            }
            
        }
    }

    private void Update()
    {
        if (transform.position.z < -destroy_behind)
            Destroy(gameObject);
    }
}
