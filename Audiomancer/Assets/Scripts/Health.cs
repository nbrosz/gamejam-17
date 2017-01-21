using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Health : MonoBehaviour {

    public AudioSource audioSource;

    public float healthPoints; // number of health points character has
    public AudioClip[] damageSounds;
    private int lastDamageSoundIndex = -1;
    public AudioClip deathSound;

    private void OnTriggerEnter(Collider col) {
        var colDamage = GetCollisionDamage(col.gameObject.name);
        if (colDamage.HasValue && colDamage.Value != 0) { // positive damage value indicates single-shot damage
            DoDamage(Mathf.Abs(colDamage.Value));
        }
    }

    private void OnTriggerStay(Collider col) {
        var colDamage = GetCollisionDamage(col.gameObject.name);
        if (colDamage.HasValue && colDamage.Value < 0) { // sustained damage indicated by negative damage
            DoDamage(Mathf.Abs(colDamage.Value));
        }
    }

    void DoDamage(float amount) {
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

    float? GetCollisionDamage(string collisionName) {
        var match = Regex.Match(collisionName, @"(?<=\[)(\+?\d+(\.\d+)?)(?=\])"); // use floats like [30] or [25.5] to do damage, or [-30] or [-25.5] for sustained damage
        if (match.Success)
            return float.Parse(match.Value);
        else
            return null;
    }
}
