using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    [SerializeField] AudioClip openingDoorSFX;
    [SerializeField] AudioClip closingDoorSFX;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerExit2D(Collider2D collision)
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
