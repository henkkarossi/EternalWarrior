using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    //Shooting controller sets these values when projectile is spawned
    public float speed;
    public int damage;

    public GameObject parent;
    string targetTag;

    Vector3 lastPosition;
    Vector3 direction;

    public bool hit;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        direction = (lastPosition - transform.position).normalized;
        lastPosition = transform.position;

        if(hit)
        {
            ReturnToParent();
            hit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(parent.tag) && !other.CompareTag("Pit"))
        {
            if (other.CompareTag(targetTag))
            {
                Vector3 knockback = direction * damage * 0.2f;

                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Enemy>().TakeDamage(damage);
                    //other.GetComponent<Enemy>().KnockBack(knockback);
                }
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<Player>().TakeDamage(damage);
                    //other.GetComponent<Player>().KnockBack(knockback);
                }    
            }
            
            hit = true;
        }
    }

    public void ReturnToParent()
    {
        if (parent != null)
        {
            if(parent.gameObject.activeSelf)
                parent.GetComponent<Shooting>().ChangeProjectileState(gameObject, false);
            else
                transform.position = new Vector3(9999, 9999, 9999);
        }    
    }

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetTargetTag(string targetTag)
    {
        this.targetTag = targetTag;
    }

    public void SetProjectileSpeed(float n)
    {
        speed = n;
    }
    public void UpgradeProjectileSpeed(float u)
    {
        speed += u;
    }
    public void SetProjectileDamage(int n)
    {
        damage = n;
    }
    public void UpgradeProjectileDamage(int u)
    {
        damage += u;
    }
}

