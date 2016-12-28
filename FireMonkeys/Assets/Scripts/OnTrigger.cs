using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class OnTrigger : MonoBehaviour {

    public UnityEvent callback;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) callback.Invoke();

    }
}
