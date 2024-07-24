using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseUnitDamageSynergy", menuName = "Custom/IncreaseUnitDamageSynergy", order = 2)]
public class IncreaseUnitDamageSynergy : Synergy
{
    private int increaseDamame = 20;

    public override void ApplySynergyToUnit(UnitAbilites ua)
    {
        ua.damage += increaseDamame;
    }

    public override void RemoveSynergyToUnit(UnitAbilites ua)
    {
        ua.damage -= increaseDamame;
    }
}
