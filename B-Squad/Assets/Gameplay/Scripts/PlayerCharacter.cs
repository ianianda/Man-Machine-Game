using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public float speed;
    CharacterController cc;
    Animator animator;
    bool isAlive = true;
    public float turnSpeed; // rotation speed.
    public Rigidbody shell; // for the bomb that throw.
    public Transform muzzle;
    public float launchForce = 10; //when throw bomb, there is a force that gives on the bomb.
    public AudioSource shootAudioSource;

    //attacking is periodic.
    //during attacking, cannot move.
    bool attacking = false;
    public float attackTime;

    float hp;
    public float hpMax = 100;
    public Slider hpSlider;
    public Image hpFillImage;
    public Color hpColorFull = Color.green;
    public Color hpColorNull = Color.red;
    public ParticleSystem explosionEffect;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        hp = hpMax;
        RefreshHealthHUD();
    }

    /*
     * param v, get the direction which the movement is going to happen
     */
    public void Move(Vector3 v) 
    {
        if(!isAlive)
        {
            return;
        }

        if(attacking)
        {
            return;
        }

        Vector3 movement = v * speed;
        cc.SimpleMove(movement); //simplemove ignore y axis. unit in m/s. return if players is touching the ground. using gravity.
                                 // move() does not use gravity, return collisonFlags.
        if(animator)
        {
            animator.SetFloat("Speed", cc.velocity.magnitude);
        }
    }
	
    public void Attack()
    {
        if(!isAlive)
        {
            return;
        }

        if(attacking)
        {
            return;
        }

        var shellInstance = Instantiate(shell, muzzle.position, muzzle.rotation) as Rigidbody; // instantiate: similar to copy.
        shellInstance.velocity = launchForce * muzzle.forward; // the bomb will go forward.

        //plays the animation, sound.
        if(animator)
        {
            animator.SetTrigger("Attack");
        }
        attacking = true;
        shootAudioSource.Play();

        //invoke function: even though the object is destroyed, it still will call that.
        Invoke("RefreshAttack", attackTime);
    }

    void RefreshAttack()
    {
        attacking = false;
    }

    /*
     * param lookDir, the direction of rotation happens.
     */
    public void Rotate(Vector3 lookDir)
    {
        //gets the front of current position 
        var targetPos = transform.position + lookDir;

        //gets the current location.
        var characterPos = transform.position;

        //gets rid of the impact of y axis.
        characterPos.y = 0;
        targetPos.y = 0;

        //gets the position where the player is facing to the target.
        var faceToTargetDir = targetPos - characterPos;

        //gets the length and direction of rotation.
        var faceToQuat = Quaternion.LookRotation(faceToTargetDir);
        //Quaternion is another way to represent rotation, like matrix.
        //a quaternion number can be represented as x = a + bi + cj + dk. i,j,k are Imaginary number.
        //quaternion has higher efficiency than matrix, avoid gimbal lock.

        //calculates the rotation assigns to each frame.
        //Spherically interpolates between two vectors.
        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, turnSpeed = Time.deltaTime);
        transform.rotation = slerp;
    }

    public void Death()
    {
        isAlive = false;

        //deletes its parent.
        explosionEffect.transform.parent = null;
        explosionEffect.gameObject.SetActive(true);

        //after playing effect then delete it.
        ParticleSystem.MainModule mainModule = explosionEffect.main;
        //after that duration, then delete it.
        Destroy(explosionEffect.gameObject, mainModule.duration);

        gameObject.SetActive(false);
    }

    /*
     * param amount, how much the damage is.
     */
    public void TakeDamage(float amount)
    {
        hp -= amount;
        RefreshHealthHUD();
        if (hp <= 0f && isAlive)
        {
            Death();
        }
    }
    public void RefreshHealthHUD() // for UI
    {
        hpSlider.value = hp;
        hpFillImage.color = Color.Lerp(hpColorNull, hpColorFull, hp / hpMax);
    }


}
