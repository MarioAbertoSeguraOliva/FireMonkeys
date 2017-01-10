using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	void Awake () {
        MonoBehaviour.DontDestroyOnLoad(gameObject);
	}
	
}
