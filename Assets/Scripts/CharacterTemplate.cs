using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTemplate : MonoBehaviour
{
    [SerializeField] private int baseDamage;
    [SerializeField] private int baseDefense;

    private bool isPlayer;

    private int health;
    private int mana;
    private int stamina;

    private PlayerMovement playerMovement;

    public void Init(int health, int mana, int stamina)
    {
        this.health = health;
        this.mana = mana;
        this.stamina = stamina;
    }

    public void AssignController(bool isPlayer, PlayerMovement playerMovement)
    {
        this.isPlayer = isPlayer;

        if (this.isPlayer)
        {
            this.playerMovement = GetComponent<PlayerMovement>();
        }
    }
}
