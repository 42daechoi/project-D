using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthProportionalSynergy", menuName = "Custom/HealthProportionalSynergy", order = 1)]
public class HealthProportionalSynergy : Synergy
{
    private float healthProportionalRate = 0.1f;

    private void OnEnable()
    {
        target = "Monster";
    }
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        float additianalDamage = monster.Hp * healthProportionalRate;
        additianalDamage += ua.damage;
        monster.Hp -= additianalDamage;
    }
}
