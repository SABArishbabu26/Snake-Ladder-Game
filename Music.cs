using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioSource music;

    public void OnMusic()
    {
        music.Play();

    }

    public void OffMusic()
    {
        music.Stop();
    }
    /*private void Awake() {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Music");
        if (musicObj.Length > 1)
        {
           Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
     }*/
}
