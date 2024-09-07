using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    public event Action<Vector3> OnMove;
    public event Action OnHold;
    public event Action OnIsAttack;
    public event Action<Vector3> OnAttack;
    public event Action<int> OnMonsterDead;
    public event Action<Monster> OnTargetAttack;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void TriggerMove(Vector3 targetPosition)
    {
        OnMove?.Invoke(targetPosition);
    }

    public void TriggerHold()
    {
        OnHold?.Invoke();
    }

    public void TriggerIsAttack()
    {
        OnIsAttack?.Invoke();
    }

    public void TriggerAttack(Vector3 targetPosition)
    {
        OnAttack?.Invoke(targetPosition);
    }
    
    public void TriggerMonsterDead(int goldReward)
    {
        OnMonsterDead?.Invoke(goldReward);
    }

    public void TriggerTargetSet(Monster currentTarget)
    {
        OnTargetAttack?.Invoke(currentTarget);
    }
}
