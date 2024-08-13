using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowSynergy", menuName = "Custom/SlowSynergy", order = 1)]
public class SlowSynergy : Synergy
{
    public float slowdownRate = 0.5f;
    public override void ApplySynergyToMonster(Monster monster, UnitAbilities ua)
    {
        monster.Speed *= slowdownRate;
    }


}
