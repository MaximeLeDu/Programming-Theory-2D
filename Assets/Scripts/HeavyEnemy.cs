using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void OnStarting()
    {
        base.OnStarting();
        speed /= 2;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    //POLYMORPHISM
    public override void ReceiveDamage(int damage, float knockbackStrength, Entity attacker)
    {
        base.ReceiveDamage(damage / 2, knockbackStrength / 2, attacker);
    }
}
