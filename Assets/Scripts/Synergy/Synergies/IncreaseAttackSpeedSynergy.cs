using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackSpeedSynergy", menuName = "Custom/IncreaseAttackSpeedSynergy", order = 2)]
public class IncreaseAttackSpeedSynergy : Synergy
{
    private float increaseSpeedRate = 0.1f;

    private void OnEnable()
    {
        target = "Unit";
    }
    public override void ApplySynergyToUnit(UnitAbilities ua, int upgrade)
    {
        if (upgrade == 1) ua.attackSpeed += increaseSpeedRate;
        if (upgrade == 2) ua.attackSpeed += increaseSpeedRate * 2;
    }

    public override void RemoveSynergyToUnit(UnitAbilities ua, int upgrade)
    {
        if (upgrade == 1) ua.attackSpeed -= increaseSpeedRate;
        if (upgrade == 2) ua.attackSpeed -= increaseSpeedRate * 2;
    }
}
