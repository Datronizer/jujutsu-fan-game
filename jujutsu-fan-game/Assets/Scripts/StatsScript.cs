using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsScript : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float mana;
    [SerializeField] private float stamina;

    [SerializeField] private TextMeshProUGUI statsHud;

    //[SerializeField] private bool isPlayer;
    //[SerializeField] CharacterTemplate characterTemplate;

    private void Awake()
    {
        UpdateHud();
    }

    private void UpdateHud()
    {
        this.statsHud.SetText($"<b>Health:</b> {this.health}\n<b>Cursed Energy:</b> {this.mana}\n<b>Stamina:</b> {this.stamina}\n");
    }

    private void __TakeDamage(float damageTaken)
    {
        this.health -= damageTaken;
        UpdateHud();
        print("Damage taken: " + damageTaken);
    }
}
