using UnityEngine;

// give power of a projectile
public class Projectile : MonoBehaviour
{
	public float damage = 20f;  // impect on hit
	public float speed = 5f;    // speed of travel, no air resistance

	public GameObject destroyEffect;    // impect effect

	private void Update()
	{
		// moving straight, [with dotween there was issue]
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}

	// handling on impect
	private void OnTriggerEnter(Collider other)
	{
		// adding damage f we hit a player
		if (other.TryGetComponent(out CharacterController3D characterController))
		{
			characterController.GetDamage(damage);
		}

		// spawning impect effect
		EffectsManager.SpawnEffect(destroyEffect, transform.position, 2f);

		// removing from scene,
		Destroy(gameObject);
	}
}
