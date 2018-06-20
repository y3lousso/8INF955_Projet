﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    private AudioSource audioSource;
    public AudioData audioData;
    public TargetCubeSpawner spawner;

    [Range(1, 10)]
    public float targetCubeSpeed = 5f;
    [Range(0, 1)]
    public float slashAngleThreshold = .5f;
    [Range(0, 1)]
    public float slashIntensityThreshold = .5f;

    public float timeBeforeNextBeat = 0f;
    public int currentIndex = 0;

    public Results results = new Results();
    public float multiplier = 1f;

    public bool isPaused = false;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    public void Play ()
    {
        results = new Results();
        spawner = FindObjectOfType<TargetCubeSpawner>();
        currentIndex = 0;

        if(audioData.startingOffset < 0f)
        {
            Invoke("StartMusic", 0f);
            AtmosphereManager.Instance.StartToggleColor(audioData.beatPerMinute, 0f);
            InvokeRepeating("StartSpawning", -audioData.startingOffset, 60f / audioData.beatPerMinute);
        }
        else
        {
            Invoke("StartMusic", audioData.startingOffset);
            AtmosphereManager.Instance.StartToggleColor(audioData.beatPerMinute, audioData.startingOffset);
            InvokeRepeating("StartSpawning", 0f, 60f / audioData.beatPerMinute);
        }
        
    }

    public void Finish()
    {
        CancelInvoke("StartSpawning");

        results.Date = System.DateTime.Today.ToString();
        results.Difficulty = audioData.difficulty;
        SceneDataManager.Instance.GoToMenuScene();
    }

    private void StartMusic()
    {
        audioSource.clip = audioData.audioClip;
        audioSource.Play();
    }

    private void StartSpawning()
    {
        if (!isPaused && audioSource.isPlaying)
        {
            spawner.SpawnTargetCubes(audioData.listCubes.FindAll(t => t.Id == currentIndex));
            spawner.SpawnWall(audioData.listWalls.FindAll(w => w.Id == currentIndex));
            currentIndex++;
        }else if(!isPaused && !audioSource.isPlaying && currentIndex > 20){
            Finish();
        }
    }

    public void Pause()
    {
        isPaused = true;
        audioSource.Pause();        
    }

    public void Unpause()
    {
        audioSource.UnPause();
        isPaused = false;
    }
}
