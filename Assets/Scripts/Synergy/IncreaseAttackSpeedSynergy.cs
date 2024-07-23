using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackSpeedSynergy", menuName = "Custom/IncreaseAttackSpeedSynergy", order = 2)]
public class IncreaseAttackSpeedSynergy : Synergy
{
    private float IncreaseSpeedRate = 0.1f;

    public override void ApplySynergyToUnit(UnitAbilites ua)
    {
        ua.attackSpeed += IncreaseSpeedRate;
    }

    public override void RemoveSynergyToUnit(UnitAbilites ua)
    {
        ua.attackSpeed -= IncreaseSpeedRate;
    }
}
