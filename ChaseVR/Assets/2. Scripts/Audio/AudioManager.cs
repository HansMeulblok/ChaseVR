using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Manager for all audio.
/// </summary>
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance = null;

	[HideInInspector] public List<AudioClip> audioClips = new List<AudioClip>();

	//Variables for audio source object pooling
	[HideInInspector] public List<GameObject> audioSourceObjects;
	public GameObject audioSourceObjectToPool;
	public int amountToPool;

	// Variables for the building and driving music 
	[HideInInspector] public AudioSource musicSource;
	public clips currentMusicClip;
	[Range(0f, 0.5f)]
	public float musicVolume;
	public float timeToFade;
	private float timeElapsed = 0f;

	// Enum used for ease of use of implementing any sound effect
	public enum clips
	{
		BeachSound,
		DominantHandAudioQueue,
		NonDominantHandAudioQueue

		/// wave sound https://www.youtube.com/watch?v=2D8pEz7eSEo, https://www.youtube.com/watch?v=T-RHIo48lPU
	};

	// Makes an instance of audiomanager to be used in other places, can be rewritten to the gamemanager instance
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		audioClips = Resources.LoadAll("Audio/", typeof(AudioClip)).Cast<AudioClip>().ToList();
		AudioListener.volume = 1;
	}

	private void Start()
	{
		// Creates the music sources and plays the clip selected as current music clip
		musicSource = FindObjectOfType<Camera>().GetComponent<AudioSource>();

		musicSource.loop = true;

		SetMusic(currentMusicClip);

		// Create the audio source object pool used in the project
		audioSourceObjects = new List<GameObject>();

		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(audioSourceObjectToPool, GameObject.Find("AudioManager").transform);
			tmp.SetActive(false);
			audioSourceObjects.Add(tmp);
		}
	}

	/// <summary>
	/// Method that gets a not used audio source object from the audio source ObjectPool
	/// </summary>
	/// <returns></returns>
	public GameObject GetPooledAudioSourceObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!audioSourceObjects[i].activeInHierarchy)
			{
				audioSourceObjects[i].SetActive(true);

				return audioSourceObjects[i];
			}
		}
		return null;
	}

	/// <summary>
	/// Play a single clip through the sound effects source.
	/// </summary>	
	public void Play(clips clipName, AudioSource source)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		source.clip = toBePlayedClip;

		WaitForEndOfSound(source.gameObject, toBePlayedClip.length);

		// Only play the sound when the source is still enabled
		if (source.isActiveAndEnabled)
			source.Play();
	}

	/// <summary>
	/// Enumerator that disables the audio source from the audioSourcesPool after the duration of the playing clip
	/// </summary>
	public IEnumerator WaitForEndOfSound(GameObject audioSourceObj, float clipDuration)
	{
		yield return new WaitForSeconds(clipDuration);

		if (audioSourceObj != null)
		{
			audioSourceObj.SetActive(false);
		}
	}


	/// <summary>
	/// Play a music clip through the looping music source.
	/// </summary>
	/// <param name="clip"></param>
	public void SetMusic(clips clipName)
	{
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		musicSource.clip = toBePlayedClip;
		musicSource.volume = musicVolume;
		musicSource.Play();

		// and starts the coroutine for fading the music
		StartCoroutine(FadeMusic(clipName, toBePlayedClip));
	}

    /// <summary>
    /// The coroutine that handles the fading in and out of the building and driving soundtracks.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="toBePlayedClip"></param>
    /// <returns></returns>
    private IEnumerator FadeMusic(clips clipName, AudioClip toBePlayedClip)
    {
        timeElapsed = 0f;

		while (timeElapsed < timeToFade)
		{
			musicSource.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);

			timeElapsed += Time.deltaTime;
			yield return null;
		}


		/*if (clipName == clips.BuildingMusic)
        {
            musicSource1.clip = toBePlayedClip;
            musicSource1.Play();

            while (timeElapsed < timeToFade)
            {
                musicSource1.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);
                musicSource2.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            musicSource2.Stop();
        }
        else if (clipName == clips.DrivingMusic)
        {
            musicSource2.clip = toBePlayedClip;
            musicSource2.Play();

            while (timeElapsed < timeToFade)
            {
                musicSource2.volume = Mathf.Lerp(0, musicVolume, timeElapsed / timeToFade);
                musicSource1.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            musicSource1.Stop();
        }*/
    }

    /// <summary>
    /// Method that gets a object from the audio source object pool and plays the menu button click sound effect.
    /// </summary>
    /*public void MenuButtonClickSound()
	{
		GameObject audioSource = AudioManager.Instance.GetPooledAudioSourceObject();
		audioSource.transform.localPosition = gameObject.transform.position;
		audioSource.SetActive(true);

		Play(clips.MenuButtonClick, audioSource.GetComponent<AudioSource>());

		StartCoroutine(WaitForEndOfSound(audioSource, audioSource.GetComponent<AudioSource>().clip.length));
	}*/

    /// <summary>
    /// Stops the sounds played by the audio source given.
    /// </summary>
    /// <param name="source"></param>
    public void Stop(AudioSource source)
	{
		source.Stop();
	}
}