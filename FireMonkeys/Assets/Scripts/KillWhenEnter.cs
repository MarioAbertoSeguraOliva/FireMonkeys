using UnityEngine;
using System.Collections;

public class KillWhenEnter : MonoBehaviour {

    public GameObject killerPrefab;
    private GameObject killEntity;

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 position = transform.position + Vector3.up * 40;
            killEntity = (GameObject)Instantiate(killerPrefab, position, killerPrefab.transform.rotation);
            killEntity.GetComponent<Moon>().objective = other.gameObject;
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
