using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour
{
    

    public void PlayGame()
    {
        SceneManager.LoadScene("SceneI");
    }

    public void PlayGameII()
    {
        SceneManager.LoadScene("SceneII");
    }

    public void QuitGame()
    {       
        Application.Quit();
    }    

    public void OpenOptions()
    {

    }

    public void CloseOptions()
    {

    }

    public AudioMixer mixer;
    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    







}
