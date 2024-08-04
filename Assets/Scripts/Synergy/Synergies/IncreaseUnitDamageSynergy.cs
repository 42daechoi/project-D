using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseUnitDamageSynergy", menuName = "Custom/IncreaseUnitDamageSynergy", order = 2)]
public class IncreaseUnitDamageSynergy : Synergy
{
    private int increaseDamage = 20;

    public override void ApplySynergyToUnit(UnitAbilities ua, int upgrade)
    {
        if (upgrade == 1) ua.damage += increaseDamage;
        if (upgrade == 2) ua.damage += increaseDamage * 2;
    }

    public override void RemoveSynergyToUnit(UnitAbilities ua, int upgrade)
    {
        if (upgrade == 1) ua.damage -= increaseDamage;
        if (upgrade == 2) ua.damage -= increaseDamage * 2;
    }
}
