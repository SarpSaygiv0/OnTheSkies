using UnityEngine;

// spawn effects on demand

public class EffectsManager : MonoBehaviour
{
	// spawn effect at position, with specific life spam to destroy after it
	public static void SpawnEffect(GameObject effect, Vector3 pos, float lifeTime)
	{
		if (effect == null) return;

		// spawning and queing for destroy
		Destroy(Instantiate(effect, pos, effect.transform.rotation), lifeTime);
	}
}
