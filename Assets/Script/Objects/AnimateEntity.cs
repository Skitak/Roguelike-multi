﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimateEntity : InanimateEntity {

    protected int life;
    protected float speed;
    protected int attack;
    protected bool isDead = false;
    protected bool canBeDamaged=true;
    protected bool canAttack = true;
    public bool stun = false;
    protected Rigidbody2D rigidb;
    protected Animator animator;
    public Vector3 direction;

    public AudioClip[] sounds;
    protected AudioSource audioSource;


    public abstract void Move(Vector2 direction);

    public virtual void DecreaseHealth(int damage)
    {
        if (canBeDamaged==true)
        {
            this.life -= damage;
        }
    }

    public virtual void Idle()
    {
        animator.SetBool("isMoving", false);
        rigidb.velocity = Vector3.zero;
    }

    public bool getCanBeDamaged()
    {
        return canBeDamaged;
    }


    public override void Use(Character user)
    {
        throw new System.NotImplementedException();
    }

    public int GetAttack()
    {
        return attack;
    }

    public AudioClip getSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                return sounds[i];
            }
        }
        return new AudioClip();
    }

}
