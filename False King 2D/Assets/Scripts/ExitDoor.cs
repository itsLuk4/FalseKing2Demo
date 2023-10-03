using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //using it for scene management

public class ExitDoor : MonoBehaviour
{

    [SerializeField] float secondsToLoad = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    // method to load next level, using scene manager
    public void StartLoadingNextLevel()
    {
        GetComponent<Animator>().SetTrigger("Close");

        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(secondsToLoad);
        //using "var" will make it change depending on which variable its given
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //this will take us to the next level
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
