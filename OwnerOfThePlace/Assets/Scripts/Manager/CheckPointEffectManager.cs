using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointEffectManager : MonoBehaviour
{
    public GameObject Idle;
    public GameObject PlayerIn;

    bool playerGotIn;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !playerGotIn)
        {
            Idle.SetActive(false);
            PlayerIn.SetActive(true);
            playerGotIn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Idle.SetActive(true);
            PlayerIn.SetActive(false);
        }
    }
}
