using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip[]	friendlyBulletHitSFX,
						enemyBulletHitSFX,
						playerDeathSFX,
						enemyDeathSFX,
						totemCopySFX,
						moneySFX;

	// play random sound from array
	public void playSound(AudioClip[] sfx) {
		GetComponent<AudioSource>().PlayOneShot(sfx[(int)Random.Range(0, sfx.Length)]);
	}
}
