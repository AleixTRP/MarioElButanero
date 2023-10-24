using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void SplashToMenu()
    {
        SceneManager.LoadScene(sceneName);
    }
}
