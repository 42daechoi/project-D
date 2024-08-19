using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "ExecutionSynergy", menuName = "Custom/ExecutionSynergy", order = 1)]
public class ExecutionSynergy : Synergy
{
    private void OnEnable()
    {
        target = "Monster";
    }
    private float executionHealthRatio = 0.1f;
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        if (monster.MaxHp * executionHealthRatio >= monster.Hp)
        {
            monster.Dead();
        }
    }
}
