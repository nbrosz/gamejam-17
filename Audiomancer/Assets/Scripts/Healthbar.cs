using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    public Image healthbar;

    private float initialHealth;
    private float currentHealth;

    void Update() {
        healthbar.fillAmount = currentHealth / initialHealth;
    }
}
