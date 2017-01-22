using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Health : MonoBehaviour {

    public bool Alive { get { return healthPoints > 0; } }

    public AudioSource audioSource;

    public float healthPoints; // number of health points character has
    public AudioClip[] damageSounds;
    private int lastDamageSoundIndex = -1;
    public AudioClip deathSound;

    void DoDamage(float amount) {
        if (Alive) {
            Debug.Log("Damaged for " + amount);
            healthPoints -= amount;
            if (healthPoints > 0) {
                PlayDamageSound();
                SendMessage("OnDamaged", SendMessageOptions.DontRequireReceiver);
            } else {
                PlayDeathSound();
                SendMessage("OnKilled", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void PlayDamageSound() {
        if (damageSounds.Length == 0)
            return;

        var clipIndexToPlay = Random.Range(0, damageSounds.Length-1);
        if (clipIndexToPlay == lastDamageSoundIndex)
            clipIndexToPlay = (clipIndexToPlay + 1) % damageSounds.Length;

        audioSource.PlayOneShot(damageSounds[clipIndexToPlay]);
        lastDamageSoundIndex = clipIndexToPlay;
    }

    void PlayDeathSound() {
        if (deathSound == null)
            return;

        audioSource.PlayOneShot(deathSound);
    }
}
