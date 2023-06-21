using System.Collections;
using UnityEngine;

// handle a weapon needs
public class Gun : MonoBehaviour
{
	public GameObject projectile;       // the object or bullet to shoot

	public Transform muzzle;            // at which point to shoot
	public GameObject muzzleFlash;      // effect for muzzle

	public float fireRate = 25f;        // projectiles per second
	private float currentFireRate = 0f; // current delay for next projectile

	public Vector2 spreadRange = new Vector2(-1f, 1f);  // recoil range

	public AudioClip shotSound;         // clip to play on shoot
	private AudioManager AudioManager;  // having a refernce to audio manager

	public bool playEffects = false;    // for non-human players, to make less pressure on CPU

	private void Start()
	{
		// resolving references
		AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
	}

	private void Update()
	{
		// maintaining fire rate
		if (currentFireRate < (1 / fireRate))
			currentFireRate += Time.deltaTime;
	}

	public void Shoot()
	{
		// check we delayed much to shoot, waiting for gun to get col. before we burn its barrel
		if (currentFireRate < (1 / fireRate))
			return;

		// setting for next shot, adding heat
		currentFireRate = 0;

		// suger
		if (playEffects)
		{
			// starting muzzle effect
			StartCoroutine(MuzzleFlashCoroutine());
			// playing shot sound
			AudioManager.Play(shotSound, 0);
		}

		// spawning projectile
		GameObject p = Instantiate(projectile, muzzle.position, muzzle.rotation);
		// adding spread or recoil
		p.transform.Rotate(new Vector3(0, Random.Range(spreadRange.x, spreadRange.y), 0));
		// giving life time to bullet
		Destroy(p, 3f);
	}

	// manages muzzle flash effect
	IEnumerator MuzzleFlashCoroutine()
	{
		muzzleFlash.SetActive(true);

		yield return new WaitForSeconds(0.1f);

		muzzleFlash.SetActive(false);
	}
}
