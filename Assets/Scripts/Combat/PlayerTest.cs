using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public GameObject monsterContainer;
    public float damage = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyRandomStatusEffect();
        }
    }

    void ApplyRandomStatusEffect()
    {
        if (monsterContainer != null)
        {
            List<Monster> monsters = new List<Monster>();
            foreach (Transform child in monsterContainer.transform)
            {
                Monster monster = child.GetComponent<Monster>();
                if (monster != null)
                {
                    monsters.Add(monster);
                }
            }

            if (monsters.Count > 0)
            {
                int randomIndex = Random.Range(0, monsters.Count);
                Monster randomMonster = monsters[randomIndex];
                randomMonster.TakeDamage(damage);
                StatusEffect statusEffect = randomMonster.GetComponent<StatusEffect>();
                if (statusEffect == null)
                {
                    statusEffect = randomMonster.gameObject.AddComponent<StatusEffect>();
                }
                statusEffect.SlowEffect(randomMonster);
            }
        }
    }
}