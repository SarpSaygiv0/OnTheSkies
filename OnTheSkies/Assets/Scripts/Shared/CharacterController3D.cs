using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// assures a gameObject is moveable and decision maker
public class CharacterController3D : MonoBehaviour
{
	public float speed = 2f;        // distance multiplier per frame
	public float health = 100f;     // starting health

	public KeyCode LeftShootKey = KeyCode.Mouse0;       // 1st weapons shoot key
	public KeyCode RightShootKey = KeyCode.Mouse1;      // 2nd weapon shoot key

	public Gun LeftGun;                                 // gun object
	public Gun RightGun;                                // gun object

	public GameObject destroyEffect;                    // on death effect

	public Text healthText;                             // health indicator, text

	private bool isDead = false;                        // [private] equal to gameOver

	public AudioClip dieSound;                          // sound on death
	private AudioManager AudioManager;                  // reference to sound player

	public bool master = false;                         // human controllable or AI

	private NavMeshAgent Agent;                         // path finder
	private Transform Target;                           // move to

	public float minimumDis = 5f;                       // stop distance from target

	// movment for both human and Ai
	private void Movement()
	{
		if (master)     // if human player
		{
			// move to
			Vector3 moveTo = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime),
										 transform.position.y,
										 transform.position.z + (Input.GetAxis("Vertical") * speed * Time.deltaTime));
			transform.DOMove(moveTo, Time.deltaTime).SetEase(Ease.Linear);  // move
			return;
		}

		// if player died. or some other reason
		if (Target == null) return;
		Vector3 dir = transform.position - Target.position;     // position vector

		if (dir.magnitude > minimumDis) // if we r away from player
		{
			Agent.isStopped = false;
			Agent.SetDestination(Target.position);  // moving towards player
		}
		else
		{
			Agent.isStopped = true; // stoping
		}
	}

	// shoting for both human and AI
	private void Shoot()
	{
		if (master) // if human
		{
			if (Input.GetKey(LeftShootKey))     // on left click
				LeftGun.Shoot();
			if (Input.GetKey(RightShootKey))    // on right click
				RightGun.Shoot();

			return;
		}

		if (Target == null) return;             // if player is dead, or any reason,

		Vector3 dir = transform.position - Target.position;
		if (dir.magnitude > minimumDis) return; // if we are far from player ,yet

		// shoot with both hands
		LeftGun.Shoot();
		RightGun.Shoot();
	}

	// look ability for both player and AI
	private void Look()
	{
		// driectio to look at
		Vector3 dir;

		if (master)     // if human
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // getting mouse postion, screen -> world space
			mousePos.y = transform.position.y;                                      // making parrallel
			dir = mousePos - transform.position;                                    // getting position vector of pointer relative to player
			transform.LookAt(dir * 10f);                                            // looking toward the position vector
																					// offseting to handle situation when pointer goes on player, or near player

			return;
		}

		// for AI

		if (Target == null) return;             // if player died, or other reasons

		transform.LookAt(Target.position);      // we have our vector, look at it.
	}

	// carefully remove health, watching some events
	public void GetDamage(float damage)
	{
		// if we died
		if ((health -= damage) <= 0)
		{
			// spawn on Death effect
			EffectsManager.SpawnEffect(destroyEffect, transform.position, 2f);
			isDead = true;                      // set internal condition

			health = 0;                         // removing excess damage, to fix stats

			if (master)                         // if human
			{
				Invoke("Restart", 3f);          // restart game after delay
				gameObject.SetActive(false);    // turn off object, if we removed it, the pending execution of game restart will be cancelled
			}
			else
				Destroy(gameObject);            // but ok, for a  AI, to be removed.

			AudioManager.Play(dieSound);        // play die sound effect
		}

		if (!master) return;                    // exclusively for human players

		healthText.text = health.ToString();    // updte health stat
		if (health < 100) healthText.color = Color.yellow;          // give quick info with health color
		if (health < 60) healthText.color = Color.red;              // give quick info with health color
	}

	// for 1st frame
	private void Start()
	{
		// resolving reference
		AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

		if (master)     // if human, then show stats
		{
			healthText.text = health.ToString();    // show health stat

			return;
		}

		// resolving references
		Agent = GetComponent<NavMeshAgent>();
		Target = GameObject.FindWithTag("Player")?.transform;
		// syncing settings to path finder
		Agent.speed = speed;
	}

	// for each frame
	private void Update()
	{
		// if we are alive, then do yuor work
		if (!isDead)
		{
			Movement();     // move
			Shoot();        // shoot
			Look();         // see

			return;         // again
		}
	}

	// restarting game
	private void Restart()
	{
		// loading 1st scene, which points to gameplay scene currently.
		SceneManager.LoadScene(0);
	}
}
