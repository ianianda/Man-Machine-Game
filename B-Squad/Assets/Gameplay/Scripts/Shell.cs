using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float explosionRadius;
    public LayerMask damageMask;
    public float damage = 20;
    public AudioSource explosionAudioSource;
    public ParticleSystem explosionEffect;
    public bool isRotate = false;

    private void Start()
    {
        Destroy(gameObject, 3.5f);
        if(isRotate)
        {
            GetComponent<Rigidbody>().AddTorque(transform.right * 1000);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //detects the area that shell will cover after shooting.
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageMask);

        //checks what is included in shooting areas
        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<PlayerCharacter>(); //players items
            if (target)
            {
                target.TakeDamage(damage);
            }
        }

        explosionAudioSource.Play();
        //in order to play this, we need to clean the parent.
        explosionEffect.transform.parent = null;
        explosionEffect.Play();

        //after playing effect then delete it.
        ParticleSystem.MainModule mainModule = explosionEffect.main;
        //after that duration, then delete it.
        Destroy(explosionEffect.gameObject, mainModule.duration);
        Destroy(gameObject);

    }

}
