﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayIngameMenu : MonoBehaviour {
    public List<Camera> cameras;
    public GameObject play, pause;
    private int currentCam = 0;
    private bool isPlaying = true;
    private float timeScaleModifier = 1;

    public void Pause()
    {
        if (isPlaying)
        {
            play.SetActive(true);
            pause.SetActive(false);
            Time.timeScale = 0;
            isPlaying = false;
        }
        else
        {
            play.SetActive(false);
            pause.SetActive(true);
            Time.timeScale = 1;
            timeScaleModifier = 1;
            isPlaying = true;
        }
    }

    public void ExitReplay()
    {
        SceneManager.LoadScene(0);
    }

    public void SpeedUp()
    {
        timeScaleModifier += 0.1f;
        if (timeScaleModifier < 2f)
        {
            timeScaleModifier = 2f;
        }
        Time.timeScale = timeScaleModifier;
    }

    public void SlowDown()
    {
        timeScaleModifier -= 0.1f;
        if (timeScaleModifier < 0.2f)
        {
            timeScaleModifier = 0.2f;
        }
        Time.timeScale = timeScaleModifier;
    }

    public void ReloadReplay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextCamera()
    {
        if (cameras.Count > 1)
        {
            foreach (Camera cam in cameras)
            {
                cam.enabled = false;
            }

            currentCam++;
            if (currentCam == cameras.Count)
            {
                currentCam = 0;
            }
            cameras[currentCam].enabled = true;
        }
    }

    public void PrviousCamera()
    {
        if (cameras.Count > 1)
        {
            foreach (Camera cam in cameras)
            {
                cam.enabled = false;
            }

            currentCam--;
            if (currentCam < 0)
            {
                currentCam = cameras.Count - 1;
            }
            cameras[currentCam].enabled = true;
        }
    }
}