using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mana;
    [SerializeField] private int stamina;

    [SerializeField] private int baseDamage;
    [SerializeField] private int baseDefense;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 100;
        this.mana = 200;
        this.stamina = 75;

        this.baseDamage = 10;
        this.baseDefense = 25;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
