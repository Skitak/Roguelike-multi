﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimateEntity : InanimateEntity
{

    protected Timer invincibility;
    public int health = 10;
    public float speed = 10;
    public int attack = 2;
    public ProtectionShield protectionShield;
    protected bool canBeDamaged = true;
    protected bool canAttack = true;
    protected bool isDead = false;
    [HideInInspector]
    public bool stun = false;
    public float timeOfInvincibility;
    protected Rigidbody2D rigidb;
    protected Animator animator;
    [HideInInspector]
    public Vector3 direction;
    protected bool isDying = false;
    protected AudioSource audioSource;
    protected float scaleMultiplier;

    private ProtectionShield currentShield=null;



    protected virtual void Start()
    {
        animator = this.GetComponent<Animator>();
        invincibility = new Timer(timeOfInvincibility, true);
        rigidb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        scaleMultiplier = 1;
    }

    public virtual void Move(Vector2 directionRequired)
    {
        this.direction = directionRequired.normalized;
        rigidb.velocity = direction * speed;
        animator.SetFloat("directionX", direction.x);
        animator.SetFloat("directionY", direction.y);
        animator.SetBool("isMoving", true);
    }
    public virtual void Idle()
    {
        animator.SetBool("isMoving", false);
        rigidb.velocity = Vector3.zero;
    }


    public override void Use(Character user)
    {
        throw new System.NotImplementedException();
    }

    public int GetAttack()
    {
        return attack;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    protected virtual void Die()
    {
        //dyingSound.Play();
    }

    public void setCanBeDamaged(bool b)
    {
        canBeDamaged = b;

        if (!canBeDamaged)
        {
            if (currentShield == null)
            {
                //sprite change de couleur indiquant impossibilité d'être frappé, bouclier posé
                currentShield = Instantiate(protectionShield, transform.position, Quaternion.identity, transform); ;
                currentShield.transform.localScale = new Vector3(transform.localScale.x + 0.3f * scaleMultiplier, transform.localScale.y + 0.3f * scaleMultiplier, 0);
                Debug.Log(this.name +" : Shield de protection activé");
            }
        }
        else
        {
            if (currentShield != null)
            {
                Destroy(currentShield.gameObject);
                currentShield = null;
                Debug.Log(this.name + " : Shield de protection désactivé");
            }
        }
    }


    public bool getCanBeDamaged()
    {
        return canBeDamaged;
    }

    public virtual void ReceiveHit(int value, GameObject other)
    {
        if (!invincibility.IsFinished())
        {
            return;
        }
        invincibility.ResetPlay();
        
        if (canBeDamaged == true &&!isDead)
        {
            
           // Debug.Log(value);
            Debug.Log("degats : " +value);
            health -= value;
            KnockBack(other);
        }
        
        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsDying()
    {
        return isDying;
    }

    private void KnockBack(GameObject other)
    {
        Vector3 knockBackDirection = (transform.position - other.transform.position).normalized;
        transform.position += knockBackDirection * (int)knockbackDistances.low;
    }

}

public enum knockbackDistances
{
    low = 1,
    medium = 2,
    high = 4
}

