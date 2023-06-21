using Unity.VisualScripting;
using UnityEngine;

// plays audio on demand
public class AudioManager : MonoBehaviour
{
	private AudioSource[] AudioSources;     // a record of all audio sources, to keep eye on
	public int channels = 10;               // maximum number of audio clips that will be handled at a time

	private void Awake()
	{
		// initializing
		AudioSources = new AudioSource[channels];
		for (int i = 0; i < channels; i++)
		{
			// adding it as component, other wise its field were remaning null.
			AudioSources[i] = transform.AddComponent<AudioSource>();
		}
	}

	// play clip on free channel
	public void Play(AudioClip clip)
	{
		// findng free channel
		for (int i = 0; i < channels; i++)
		{
			// got free channel
			if (AudioSources[i].isPlaying == false)
			{
				// playing
				AudioSources[i].clip = clip;
				AudioSources[i].Play();

				break;
			}
		}
	}

	// force to play clip on specific channel
	public void Play(AudioClip clip, int channel)
	{
		// playing
		AudioSources[channel].clip = clip;
		AudioSources[channel].Play();
	}
}
