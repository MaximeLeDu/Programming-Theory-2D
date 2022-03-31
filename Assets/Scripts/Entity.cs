using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Entity : MonoBehaviour
{
    //ENCAPSULATION
    public int Health { get; protected set; }

    protected int AttackMin;
    protected int AttackMax;
    protected int ArmorValue;

    protected float speed;
    protected float range;

    private bool isTargetable = true;

    private readonly int numberOfBlinks = 10;
    protected readonly float knockbackDuration = 1;

    private readonly float blinkingRate = 0.1f;
    protected float knockbackSpeed = 20;
    protected float knockbackStrength = 5;

    protected SpriteRenderer entitySprite;
    protected NavMeshAgent m_Agent;

    private void Start()
    {
        Debug.Log("Entity" + gameObject);
        entitySprite = GetComponent<SpriteRenderer>();

        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        m_Agent.updateUpAxis = false;
        m_Agent.acceleration = 999;

        OnStarting();
        m_Agent.speed = speed; // speed is defined on the OnStarting class
    }

    protected abstract void OnStarting();

    private void Update()
    {
        OnUpdate();
    }

    protected abstract void OnUpdate();

    public virtual void Attack(Entity entity)
    {
        if(DistanceToEntity(entity) < range)
        {
            int damage = Random.Range(AttackMin, AttackMax);
            entity.ReceiveDamage(damage, knockbackStrength, this);
        }
    }

    public virtual void ReceiveDamage(int damage, float knockbackStrength, Entity attacker)
    {
        //If entity has received a hit and is on invulnerability frame, don't receive damage
        if (!isTargetable) { return; }
        int realDamage;
        if (ArmorValue >= damage) { realDamage = 1; }
        else { realDamage = damage - ArmorValue; }
        Health -= realDamage;
        if (Health <= 0)
        {
            Die();
            return;
        }
        //If entity is not dead, it is knockbacked and is invulnerable for some frames
        isTargetable = false;
        Vector2 knockback = (transform.position - attacker.transform.position).normalized * knockbackStrength;
        Knockback(knockback);
        StartCoroutine(InvulnerabilityBlink());
    }

    public virtual void MoveTowards(Vector2 direction)
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    //ABSTRACTION
    public virtual void MoveTo(Vector2 position)
    {
        m_Agent.SetDestination(position);
        m_Agent.isStopped = false;
    }

    //POLYMORPHISM
    public virtual void MoveTo(Entity entity)
    {
        m_Agent.SetDestination(entity.transform.position);
        m_Agent.isStopped = false;
    }

    protected virtual void Knockback(Vector2 knockback)
    {
        StartCoroutine(KnockbackRoutine(knockback));
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public float DistanceToEntity(Entity entity)
    {
        return Vector2.Distance(transform.position, entity.transform.position);
    }

    IEnumerator InvulnerabilityBlink()
    {
        int currentNumberBlinks = 0;
        while(currentNumberBlinks < numberOfBlinks)
        {
            currentNumberBlinks += 1;
            entitySprite.enabled = !entitySprite.enabled;
            yield return new WaitForSeconds(blinkingRate);
        }
        isTargetable = true;
        yield return null;
    }

    IEnumerator KnockbackRoutine(Vector2 knockback)
    {
        Vector2 initialPos = transform.position;
        float travel = 0;
        while(travel < 1)
        {
            travel += Time.deltaTime * (1/knockbackDuration);
            Vector2 pos = Vector2.Lerp(initialPos, knockback, travel);
            transform.position = pos;
            yield return null;
        }
        yield return null;
    }
}
