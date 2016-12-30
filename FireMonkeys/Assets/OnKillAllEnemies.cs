using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class OnKillAllEnemies : MonoBehaviour {

    int aliveEnemies;
    public UnityEvent onKillAllEnemies;

	// Use this for initialization
	void Start () {
        Health[] enemiesHealth = transform.GetComponentsInChildren<Health>();
        aliveEnemies = enemiesHealth.Length;
        foreach (var health in enemiesHealth)
        {
            health.onChangeHealthEvent += EnemyDamage;
        }
	}
	
    public void EnemyDamage(float life)
    {
        if (life == 0)
        {
            aliveEnemies--;
            if (aliveEnemies == 0) onKillAllEnemies.Invoke();

        }
    }

}
