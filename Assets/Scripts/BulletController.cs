using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRB;

    public Vector2 moveDir;

    public GameObject impactEffect;

    public int damageAmount = 1;

    void Start()
    {
        damageAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = moveDir * bulletSpeed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        Debug.Log(damageAmount);
        if (other.tag == "Enemy")
        {
            //damageAmount = 1; /// not sure why this is needed but it definitely is
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (other.tag == "Boss")
        {
            //damageAmount = 1;
            BossHealthController.instance.TakeDamage(damageAmount);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        //AudioManager.instance.PlaySFXAdjusted(3);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
