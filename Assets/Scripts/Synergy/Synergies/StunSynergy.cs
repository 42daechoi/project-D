using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "StunSynergy", menuName = "Custom/StunSynergy", order = 1)]
public class StunSynergy : Synergy
{
    public float originalSpeed;

    private void OnEnable()
    {
        target = "Monster";
    }
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        originalSpeed = monster.Speed;
        monster.Speed = 0f;
    }
    public override void RemoveSynergyToMonster(Monster monster, UnitAbilities ua)
    {
        monster.Speed = originalSpeed;
    }
}
