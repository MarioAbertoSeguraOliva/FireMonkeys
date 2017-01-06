using UnityEngine;
using System.Collections;

public class HealthItem : MonoBehaviour {
    public float health = 25;
    public AudioClip clip;

    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Amount += health;
            //GetComponent<Renderer>().enabled = false;
            //GetComponent<SphereCollider>().enabled = false;
            BackgroundMusic.instance.GetComponent<AudioSource>().PlayOneShot(clip);
            gameObject.SetActive(false);
        }
    }

}
