using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//INHERITANCE
public class Player : Entity
{

    public Transform allExceptPlayer;

    private bool canMove = true;
    protected override void OnStarting()
    {
        Health = 20;
        AttackMin = 3;
        AttackMax = 8;
        ArmorValue = 4;
        range = 2.5f;
        speed = 5;
    }

    protected override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Attack();
        }

        if (canMove)
        {
            allExceptPlayer.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime * Vector2.left);
            allExceptPlayer.Translate(Input.GetAxis("Vertical") * speed * Time.deltaTime * Vector2.down);
        }
    }

    public Enemy FindClosestEnemy()
    {
        Enemy[] gos;
        gos = FindObjectsOfType<Enemy>();
        if(gos == null) { return null; }
        Enemy closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Enemy go in gos)
        {
            float curDistance = DistanceToEntity(go);
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void Attack()
    {
        Enemy closest = FindClosestEnemy();
        if(closest == null) { return; }
        Attack(closest);
    }

    protected override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected override void Knockback(Vector2 knockback)
    {
        StartCoroutine(KnockbackRoutine(knockback));
    }

    IEnumerator KnockbackRoutine(Vector2 knockback)
    {
        canMove = false;
        Vector2 initialPos = allExceptPlayer.position;
        Vector2 realKnockback = initialPos - knockback;
        float travel = 0;
        while (travel < 1)
        {
            travel += Time.deltaTime * (1 / knockbackDuration);
            Vector2 pos = Vector2.Lerp(initialPos, realKnockback, travel);
            allExceptPlayer.position = pos;
            yield return null;
        }
        canMove = true;
        yield return null;
    }
}
