using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

    public float damagePerSecond = 2.5f;
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
        LayerMask otherMask = 1 << other.gameObject.layer;
        if (attackEvent != null && otherMask == victimLayer)
            attackEvent.Invoke(true);
    }

    void OnTriggerExit(Collider other)
    {
        LayerMask otherMask = 1 << other.gameObject.layer;
        if (attackEvent != null && otherMask == victimLayer)
            attackEvent.Invoke(false);
    }

    void OnTriggerStay(Collider other)
    {
        LayerMask otherMask = 1 << other.gameObject.layer;
        if (otherMask == victimLayer)
        {
            other.gameObject.GetComponentInParent<Health>().Amount -= damagePerSecond * Time.deltaTime;
        }
    }


    void OnCollisionEnter(Collision other)
    {
        OnTriggerEnter(other.collider);
    }

    void OnCollisionExit(Collision other)
    {
        OnTriggerExit(other.collider);
    }

    void OnCollisionStay(Collision other)
    {
        OnTriggerStay(other.collider);
    }
}
