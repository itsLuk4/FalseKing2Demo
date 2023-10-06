using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
