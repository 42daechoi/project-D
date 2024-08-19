using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowSynergy", menuName = "Custom/SlowSynergy", order = 1)]
public class SlowSynergy : Synergy
{
    public float slowdownRate = 0.5f;
    public float originalSpeed;

    private void OnEnable()
    {
        target = "Monster";
    }
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        originalSpeed = monster.Speed;
        monster.Speed *= slowdownRate;
    }
    public override void RemoveSynergyToMonster(Monster monster, UnitAbilities ua)
    {
        monster.Speed = originalSpeed;
    }
}
