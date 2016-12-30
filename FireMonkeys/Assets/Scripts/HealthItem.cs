using UnityEngine;
using System.Collections;

public class HealthItem : MonoBehaviour {
    public float health = 25;

    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Amount += health;
            gameObject.SetActive(false);
        }
    }
}
