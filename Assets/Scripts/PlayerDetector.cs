using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour {

    public delegate void OnDetectPlayer(GameObject detectPlayer);
    public event OnDetectPlayer detectPlayerEvent;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayerEvent.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayerEvent.Invoke(null);
        }
    }

}
