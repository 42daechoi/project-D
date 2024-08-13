using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "StunSynergy", menuName = "Custom/StunSynergy", order = 1)]
public class StunSynergy : Synergy
{
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        monster.Speed = 0f;
    }
}
