using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Health health;
    private GameObject background;
    private GameObject bar;
    private Vector3 barInitialScale;

    // Use this for initialization
    void Start () {
        health.onChangeHealthEvent += OnChangeHealth;
        background = transform.FindChild("Background").gameObject;
        bar = transform.FindChild("Bar").gameObject;
        barInitialScale = bar.GetComponent<RectTransform>().localScale;
    }
	
    void OnChangeHealth(float amount)
    {
        float maxHealth = health.maxHealth;
        bar.GetComponent<RectTransform>().localScale = 
            new Vector3(amount / maxHealth * barInitialScale.x, barInitialScale.y, barInitialScale.z);
    }

	// Update is called once per frame
	void Update () {
	    
	}
    
}
