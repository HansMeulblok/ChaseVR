using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
	[HideInInspector] public AudioSource musicSource1;
	[HideInInspector] public AudioSource musicSource2;
	public clips currentMusicClip;
	[Range(0f, 0.5f)]
	public float musicVolume;
	public float timeToFade;
	private float timeElapsed = 0f;

	[Header("Saegull")]
	public float intervalRangeMin;
	public float intervalRangeMax;
	private AudioSource seagullSource;
	[HideInInspector]
	public bool inDoors = false;

	// Enum used for ease of use of implementing any sound effect
	public enum clips
	{
		BeachSound,
		ChaseBuildingMusic,
		DominantHandAudioQueue,
		NonDominantHandAudioQueue,
		SurfMusic,
		Seagull

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
		musicSource1 = FindObjectOfType<Camera>().GetComponent<AudioSource>();
		musicSource2 = GetComponent<AudioSource>();

		musicSource1.loop = true;

		SetMusic(currentMusicClip);

		seagullSource = GameObject.Find("Seagull Sound").GetComponent<AudioSource>();

		// Create the audio source object pool used in the project
		audioSourceObjects = new List<GameObject>();

		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(audioSourceObjectToPool, GameObject.Find("AudioManager").transform);
			tmp.SetActive(false);
			audioSourceObjects.Add(tmp);
		}

	 StartCoroutine(SeagullSoundEffect());
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
		if (SceneManager.GetActiveScene().name == "SurfExperience")
        {
			SurfExperienceMusic(clipName);
			return;
        }

		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

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

        if (clipName == clips.BeachSound)
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
        else if (clipName == clips.ChaseBuildingMusic)
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
        }
    }

    
    private void SurfExperienceMusic(clips clipName)
    {
		// Checks and matches the enum in the method parameter to one of the clips in the Resource/Sounds/ folder.
		AudioClip toBePlayedClip = audioClips.Where(clip => clip.name.Contains(clipName.ToString())).FirstOrDefault();

		musicSource2.clip = toBePlayedClip;
		musicSource2.volume = musicVolume * 3f;
		musicSource2.Play();


		musicSource1.clip = audioClips.Where(clip => clip.name.Contains(clips.SurfMusic.ToString())).FirstOrDefault();
		musicSource1.volume = musicVolume;
		musicSource1.Play();
	}


	public IEnumerator SeagullSoundEffect()
    {
		seagullSource.clip = audioClips.Where(clip => clip.name.Contains(clips.Seagull.ToString())).FirstOrDefault();

		float seconds = Random.Range(intervalRangeMin, intervalRangeMax);
		float pitch = Random.Range(0.7f, 1.3f);

		yield return new WaitForSeconds(seconds);

		if (inDoors)
			seagullSource.volume = 0.2f;
		else
			seagullSource.volume = 1f;

		seagullSource.pitch = pitch;
		seagullSource.Play();

		StartCoroutine(SeagullSoundEffect());
    }


	/// <summary>
	/// Stops the sounds played by the audio source given.
	/// </summary>
	/// <param name="source"></param>

	public void Stop(AudioSource source)
	{
		source.Stop();
	}
}