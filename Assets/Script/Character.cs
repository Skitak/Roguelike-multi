﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : AnimateEntity {

	public float speed = 10;
	public Vector2 direction;
	public InanimateEntity[] inventory;
	public ArrayList ground;


	private Rigidbody2D rigidb;
	private Animator animator;
    private float primaryTimer;
    private float secondaryTimer;
    private bool isCarrying = false;
    private InanimateEntity carriedObject;
    private float deathTime;
    private float deathTimeCount;
    private int startLife;
    private bool deathAudioHasPlayed;
    private float opacityValue;
    private float blinkTime;
    private float blinkTimeCount;
    private GlobalHealthManager globalHealthManager;

    void Start () {
        globalHealthManager = FindObjectOfType<GlobalHealthManager>();
        speed = 10;
        life = 10;
        attack = 1;
        primaryTimer=0;
        secondaryTimer=0;
        inventory = new InanimateEntity [2];
		ground = new ArrayList();
		rigidb = this.GetComponent<Rigidbody2D> ();
		//this.GetComponent<SpriteRenderer>().color = new Color (Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		animator = this.GetComponent<Animator> ();
        deathTime = 5;
        deathTimeCount = 0;
        startLife = life;
        audioSource = gameObject.GetComponent<AudioSource>();
        deathAudioHasPlayed = false;
        opacityValue = 0.3f;
        blinkTime = 0.2f;
        blinkTimeCount = 0;

    }

	public override void Move(Vector2 direction){
    	this.direction = direction.normalized; 
		rigidb.velocity = direction * speed;
		animator.SetFloat ("directionX", direction.x);
		animator.SetFloat ("directionY", direction.y);
		animator.SetBool ("isMoving", true);
	}

    public void Update() // déséquiper pour l'instant
    {
        //death

        //bouton pour faire mourir le joueur (POUR LES TESTS)
        if (Input.GetKeyDown("2"))
        {
            DecreaseHealth(startLife);
        }


       if(life<=0)
        {
            if (isDead == false)
            {
                globalHealthManager.globalHealth--;
            }
            isDead = true;
            canBeDamaged = false;
            canAttack = false;
            
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, opacityValue);

            //joue le son de mort
            if (deathAudioHasPlayed == false)
            {
                audioSource.PlayOneShot(getSound("goule2Mort2"));
                deathAudioHasPlayed = true;
            }

            //temps d'invincibilité
            deathTimeCount += Time.deltaTime;
            blinkTimeCount += Time.deltaTime;

            //RESPAWN
            if (deathTimeCount >= deathTime)
            {
                isDead = false;
                canAttack = true;
                canBeDamaged = true;
                deathTimeCount = 0;
                life = startLife;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
                deathAudioHasPlayed = false;
                audioSource.PlayOneShot(getSound("respawnSound"));
                opacityValue = 0.3f;
            }
            //clignotement avant de respawn
            else if (deathTimeCount >= deathTime*0.75f)
            {
                if (blinkTimeCount >= blinkTime)
                {
                    if (opacityValue==1f)
                    {
                        opacityValue = 0.3f;
                        blinkTimeCount = 0;
                    }
                    else
                    {
                        opacityValue = 1f;
                        blinkTimeCount = 0;
                    }
                }
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, opacityValue);
            }
            

            
        }

        //ramassage puis lancer
        if (Input.GetButtonDown("Keyboard 2 interact")/*|| Input.GetButtonDown("Keyboard 1 interact")*/)
        {
            Debug.Log("gg");
            Interract();
        }

        //déséquipement arme 1
        if (Input.GetButton("Keyboard 2 primary"))
        {
            Debug.Log("gg");
            primaryTimer += Time.deltaTime;
            if (primaryTimer > 2 && inventory[0] != null)
                Carry(inventory[0]);
        }
        if (Input.GetButtonUp("Keyboard 2 primary"))
            primaryTimer = 0;

        //desequipement arme 2
        if (Input.GetButton("Keyboard 2 secondary"))
        {
            Debug.Log("gg");
            secondaryTimer += Time.deltaTime;
            if (secondaryTimer > 2 && inventory[1]!=null)
                Carry(inventory[1]);
        }
        if (Input.GetButtonUp("Keyboard 2 secondary"))
            secondaryTimer = 0;
    }
	public IEnumerator ReceiveHit (){
        if(canBeDamaged)
        {
            canBeDamaged = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(1);          
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            canBeDamaged = true;
        }
	}

    //atack
	public void InputAction (params object[] args) {	//pas fan de ce nom
		int item = (int) args[0];
        if (canAttack)
        {
            if (inventory[item] == null)
            {
                TryPickupItem(item);
            }
            else
            {
                inventory[item].Use(this);
            }
        }
        
		Debug.Log("pressed");

	}

	public float GetRotation(){
		float rotation = Vector2.Angle(new Vector2(0,1), this.direction);
			if (direction.x < 0)
				rotation *= -1;
		return rotation ;
	}

	private void TryPickupItem(int slot){
		if(ground.Count != 0 ){
			inventory[slot] = (InanimateEntity) ground[0];
			inventory[slot].Equip(this);
		}	//No need to remove the item from the ground list, it's done in OnTriggerExit
	}

    private void TryCarry()
    {
        if (ground.Count != 0)
        {
            carriedObject = (InanimateEntity)ground[0];
            Carry(carriedObject);
        }   //No need to remove the item from the ground list, it's done in OnTriggerExit
    }

    public void Interract () {
        //Si un objet interractible est sous ses pieds, alors interragis avec.
        if (isCarrying)
        {
            Throw();
        }
        else
        {
            TryCarry();
        }
    }

    public void Carry(InanimateEntity Ientity)
    {
        if(Ientity == inventory[0])
        {
            Ientity.Unequip(0);
        }
        if (Ientity == inventory[1])
        {
            Ientity.Unequip(1);
        }
        if (Ientity.tag == "Player" || Ientity.tag == "ennemi")
        {
            Ientity.GetComponent<AnimateEntity>().stun = true;
        }
        isCarrying = true;
        Debug.Log("blub");
        Ientity.GetComponentInChildren<CircleCollider2D>().enabled = false;
        Ientity.pickupCollider.enabled = false;
        Ientity.GetComponentInChildren<SpriteRenderer>().enabled = true;
        Ientity.transform.parent = this.transform;
        Ientity.transform.localPosition = Vector3.zero;
        Ientity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        carriedObject = Ientity;
    }

    public void Throw()
    {
        Debug.Log("bloub");
        if (isCarrying)
        {
            Debug.Log("blib");
            if (carriedObject.tag == "Player" || carriedObject.tag == "ennemi")
            {
                carriedObject.GetComponent<AnimateEntity>().stun = false;
            }
            carriedObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 50, ForceMode2D.Impulse);
            //carriedObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
            carriedObject.pickupCollider.enabled = true;
            carriedObject.transform.parent = null;
            //carriedObject.transform.localPosition = Vector3.zero;
            //carriedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            carriedObject = null;
            isCarrying = false;
        }
    }

    public override void  Idle() {
		animator.SetBool ("isMoving", false);
		rigidb.velocity = Vector3.zero;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag.Equals("item")){
			ground.Add(other.GetComponent<InanimateEntity>());
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag.Equals("item")){
			ground.Remove(other.GetComponent<InanimateEntity>());
		}
	}
}
