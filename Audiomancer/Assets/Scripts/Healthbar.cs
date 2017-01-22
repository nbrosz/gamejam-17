using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    public Image healthbar;
    private Health healthScript;

    private float initialHealth;
    private float currentHealth;

    void Start() {
        healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        initialHealth = currentHealth = healthScript.healthPoints;
    }

    void Update() {
        currentHealth = healthScript.healthPoints;
        healthbar.fillAmount = currentHealth / initialHealth;
    }
}
