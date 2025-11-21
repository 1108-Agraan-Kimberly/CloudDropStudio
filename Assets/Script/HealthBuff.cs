using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthBuff", menuName = "PowerUps/healthBuff")]
public class HealthBuff : PowerUpEffect
{
    public float amount = 3;

    public override void Apply(GameObject target)
    {
        target getComponent<Health>().health.value += amount;
    }
}

