using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour, IStatusEffect
{
    public void SlowEffect(Monster target)
    {
        StartCoroutine(SlowCoroutine(target));
    }

    public void StunEffect(Monster target)
    {
        StartCoroutine(StunCoroutine(target));
    }

    private IEnumerator SlowCoroutine(Monster target)
    {
        float originalSpeed = target.Speed;
        target.Speed *= 0.5f; // 50% slow
        yield return new WaitForSeconds(5f); // slow effect duration
        target.Speed = originalSpeed;
    }

    private IEnumerator StunCoroutine(Monster target)
    {
        float originalSpeed = target.Speed;
        target.Speed = 0f; // Stun
        yield return new WaitForSeconds(3f); // stun effect duration
        target.Speed = originalSpeed;
    }
}