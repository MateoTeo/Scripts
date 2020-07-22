using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuV2 : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private bool CzyJestZatrzymana;
    private void Update()
    {
         
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CzyJestZatrzymana = !CzyJestZatrzymana;
        }

        if (CzyJestZatrzymana)
        {
            ActivateMenu();
        }

        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        CzyJestZatrzymana = false;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(true);

    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public AudioMixer mixer;
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
}
