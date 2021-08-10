using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundsOnStart : MonoBehaviour
{
    public List<string> StartSounds = new List<string>();
    public List<string> StopSounds = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(string s in StartSounds)
        {
            FindObjectOfType<AudioManager>().Play(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string s in StopSounds)
        {
            if (FindObjectOfType<AudioManager>().IsPlaying(s))
            {
                FindObjectOfType<AudioManager>().Stop(s);
            }
        }
    }
}
