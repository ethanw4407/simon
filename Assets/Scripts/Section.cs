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
        doors,

    }

    public type section_type;


    private void OnTriggerEnter(Collider other)
    {
        PlayerController player_check = other.transform.parent.GetComponent<PlayerController>();

        if (player_check != null)
        {

            GameManager.instance.TraversedSection(transform);

            switch (section_type) {
                case type.basic:
                    break;
                case type.turn:
                    player_check.StartTurn(transform);
                    break;

            }
            
        }

        var animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("OPEN");
        }
    }

    private void Update()
    {
        if (transform.position.z < -destroy_behind)
            Destroy(gameObject);
    }
}
