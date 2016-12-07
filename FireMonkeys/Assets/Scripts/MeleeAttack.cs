using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

    float damagePerSecond = 10;
    LayerMask victimLayer;

	// Use this for initialization
	void Start () {
        LayerMask thisMask = 1 << gameObject.layer;
        if (thisMask == LayerMask.GetMask("Enemy"))
            victimLayer = LayerMask.GetMask("Player");
        else if (thisMask == LayerMask.GetMask("Player"))
            victimLayer = LayerMask.GetMask("Enemy");

    }
	

    void OnCollisionStay(Collision other)
    {
        LayerMask otherMask = 1 << other.collider.gameObject.layer;
        if (otherMask == victimLayer)
            other.gameObject.GetComponent < Health >().Amount -= damagePerSecond * Time.deltaTime;
    }

}
