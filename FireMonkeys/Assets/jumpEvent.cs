using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class JumpEvent : MonoBehaviour {

    public float destroyTimeout = 4f;
    public AudioClip backgroundClip;

    public void Jump(float force)
    {
        GetComponent<Animator>().SetBool("Jump", true);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(Vector3.up * force);
        if (backgroundClip != null)
            BackgroundMusic.instance.Play(backgroundClip);

        Destroy(gameObject, destroyTimeout);
    }

}
