using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

    float damagePerSecond = 10;
    LayerMask victimLayer;
    public delegate void AttackEvent(bool canClimb);
    public event AttackEvent attackEvent;

    // Use this for initialization
    void Start () {
        LayerMask thisMask = 1 << gameObject.layer;
        if (thisMask == LayerMask.GetMask("Enemy"))
            victimLayer = LayerMask.GetMask("Player");
        else if (thisMask == LayerMask.GetMask("Player"))
            victimLayer = LayerMask.GetMask("Enemy");

    }

    void OnTriggerEnter(Collider other)
    {
        if (attackEvent != null)
            attackEvent.Invoke(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (attackEvent != null)
            attackEvent.Invoke(false);
    }

    void OnTriggerStay(Collider other)
    {
        LayerMask otherMask = 1 << other.gameObject.layer;
        if (otherMask == victimLayer)
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.GetComponentInParent<Health>().Amount -= damagePerSecond * Time.deltaTime;
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (attackEvent != null)
            attackEvent.Invoke(true);
    }

    void OnCollisionExit(Collision other)
    {
        if (attackEvent != null)
            attackEvent.Invoke(false);
    }

    void OnCollisionStay(Collision other)
    {
        OnTriggerStay(other.collider);
    }
}
