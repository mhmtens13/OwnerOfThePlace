using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinterScene : MonoBehaviour
{
    public Collider2D cod;
    public int nextlevelcode;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D cod)
    {
        nextlevel(nextlevelcode);
    }
    void nextlevel(int x)
    {
        SceneManager.LoadScene(x);
    }
}
