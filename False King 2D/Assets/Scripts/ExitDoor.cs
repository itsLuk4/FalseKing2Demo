using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //using it for scene management

public class ExitDoor : MonoBehaviour
{

    [SerializeField] AudioClip openingDoorSFX;
    [SerializeField] AudioClip closingDoorSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Close");
    }

    // method to load next level, using scene manager
    public void StartLoadingNextLevel()
    {
        Invoke("closeDoor", 1);
        StartCoroutine(LoadNextLevel());
        
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2);
        //using "var" will make it change depending on which variable its given
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //this will take us to the next level
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void closeDoor()
    {
        GetComponent<Animator>().SetTrigger("Close");

    }

    void playOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position);
    }

    void playClosingDoorSFX()
    {
        AudioSource.PlayClipAtPoint(closingDoorSFX, Camera.main.transform.position);
    }
}
