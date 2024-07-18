using System.Collections;
using System.Collections.Generic;
using projectD;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthProportionalSynergy", menuName = "Custom/HealthProportionalSynergy", order = 1)]
public class HealthProportionalSynergy : ScriptableObject, ISynergy
{
    private float healthProportionalRate;

    public void ApplySynergy(UnitAbilites ua)
    {
        ua.damage += healthProportionalRate;
        Debug.Log("HealthProportionalSynergy");
        // 현재 웨이브의 몬스터 체력 정보가 필요
    }
}
