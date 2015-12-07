using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip[]	bulletHitSFX,
						playerDeathSFX,
						enemyDeathSFX;

	// play random sound from array
	void playSound(AudioClip[] sfx) {
		GetComponent<AudioSource>().PlayOneShot(sfx[(int)Random.Range(0, sfx.Length)]);
	}

	public void bulletHit() { playSound(bulletHitSFX); }
	public void playerDeath() { playSound(playerDeathSFX); }
	public void enemyDeath() { playSound(enemyDeathSFX); }
}
