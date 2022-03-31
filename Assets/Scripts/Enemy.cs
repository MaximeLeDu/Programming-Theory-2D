using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class Enemy : Entity
{
    protected Player player;

    
    protected override void OnStarting()
    {
        Health = 10;
        AttackMin = 5;
        AttackMax = 10;
        ArmorValue = 2;
        range = 1.2f;
        speed = 3;
        player = FindObjectOfType<Player>();
    }

    protected override void OnUpdate()
    {
        MoveTo(player);
        Attack(player);
    }
}
