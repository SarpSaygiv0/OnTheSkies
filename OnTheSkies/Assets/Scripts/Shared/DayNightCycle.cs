using UnityEngine;
// makes day and night and cycle between them
public class DayNightCycle : MonoBehaviour
{
	private void Update()
	{
		// rotate with time
		transform.Rotate(new Vector3(Time.deltaTime, 0f, 0f));
	}
}
