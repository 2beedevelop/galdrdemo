using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D theRB;

    public int damageAmount;
    public GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = transform.position - PlayerHealthController.instance.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player detected. Attempting to damage player...");

            if (PlayerHealthController.instance != null)
            {
                PlayerHealthController.instance.DamagePlayer(damageAmount);
                Debug.Log("Player successfully damaged.");
            }
            else
            {
                Debug.LogError("PlayerHealthController.instance is null!");
            }
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
