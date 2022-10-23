using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mana;
    [SerializeField] private int stamina;

    [SerializeField] private bool isPlayer;
    [SerializeField] CharacterTemplate characterTemplate;

    //private PlayerMovement playerMoverment;

    // Start is called before the first frame update
    void Start()
    {
        //this.playerMoverment = GetComponent<PlayerMovement>();

        this.health = 100;
        this.mana = 200;
        this.stamina = 75;

        //this.baseDamage = 10;
        //this.baseDefense = 25;
    }

    void Awake()
    {
        //this.characterTemplate.Init(health, mana, stamina);
        //this.characterTemplate.AssignController(isPlayer, playerMoverment);
    }
}
