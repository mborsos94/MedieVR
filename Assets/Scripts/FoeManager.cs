using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeManager : MonoBehaviour {
    public float timeBetweenAttacks = 4;
    public float rotationSpeed = 3;
    public float attackSpeed = 1;
    public float stop = 0;
    public float moveSpeed = 3;
    public float currentHealth = 100;
    private GameObject target;
    private bool isWalking = false;
    private float currWaitTime = 0;
    private bool isDead = false;

	// Use this for initialization
	void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player");
	}

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            KillFoe();
        }
        else
        {
            FoeLogic();
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.gameObject.tag == "Arrow")
        {
            c.transform.SetParent(this.transform.parent, false);
            c.rigidbody.isKinematic = true;
            Destroy(c.gameObject, 5);

            float distance = Vector3.Distance(c.contacts[0].point, transform.position);

            if (distance < 0.2f)
            {
                TakeDamage(25);
                //Blood spill
                //Instantiate(BullseyeEffect, transform.position, transform.rotation);
            }
            else
            {
                TakeDamage(15);
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "PlayerMelee")
        {
            TakeDamage(30);
        }
    }

    public void TakeDamage(int amount)
    {
        // Reduce the current health by the damage amount.
        if(!isDead)
        {
            currentHealth -= amount;
            currWaitTime = 0;
            //this.GetComponent<Animator>().Play("Hit");
        }

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... you must die.
            isDead = true;
        }
    }

    private void KillFoe()
    {
        if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            GameStats.totalEnemiesKilled++;
            GameStats.remainingEnemies--;
            this.GetComponent<Animator>().Play("Death");
        }
        else if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death") && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length < GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void FoeLogic()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        bool wasWalking = isWalking;

        if (!GameStats.isPlayerDead)
        {
            if (distance > stop)
            {
                isWalking = true;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(target.transform.position - this.transform.position), rotationSpeed * Time.deltaTime);

                //move towards the player
                float oldYPos = this.transform.position.y;
                this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
                this.transform.position = new Vector3(this.transform.position.x, oldYPos, this.transform.position.z);
            }
            else
            {
                isWalking = false;
            }

            if (wasWalking != isWalking && isWalking == true)
            {
                this.GetComponent<Animator>().Play("Walk");
            }
            else if (distance <= stop)
            {
                if (currWaitTime < timeBetweenAttacks)
                {
                    if ((GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length < GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime &
                        GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack")) ||
                        GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                        GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                    {
                        this.GetComponent<Animator>().Play("Idle");
                    }

                    if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        currWaitTime = currWaitTime + Time.deltaTime * attackSpeed;
                    }
                }
                else
                {
                    this.GetComponent<Animator>().Play("Attack");
                    currWaitTime = 0;
                }
            }
        }
        else
        {
            this.GetComponent<Animator>().Play("Idle");
        }
    }
}
