using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class KillWhenEnter : MonoBehaviour {

    public GameObject killerPrefab;
    public float upOffset = 25f;
    private GameObject killEntity;

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 position = other.transform.position + Vector3.up * upOffset;
            killEntity = (GameObject)Instantiate(killerPrefab, position, killerPrefab.transform.rotation);
            killEntity.GetComponent<Moon>().objective = other.gameObject;
            killEntity.GetComponent<Moon>().explosionSound = GetComponent<AudioSource>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(killEntity != null)
                Destroy(killEntity);
        }
    }

}
