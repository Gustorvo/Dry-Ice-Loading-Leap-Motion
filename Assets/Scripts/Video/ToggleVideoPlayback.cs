using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class ToggleVideoPlayback : MonoBehaviour
{
    public UnityEvent OnPlaybackFinish = new UnityEvent();
    VideoPlayer player;
    Task activePlayingRoutine;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayVideo(float delay)
    {
        if (player.isPlaying)
        {
            StopVideo();
        }
        else
            activePlayingRoutine = new Task(Play(delay), true);
        //StartCoroutine(Play(delay));
    }

    public void StopVideo()
    {
        if (player.isPlaying)
        {
            player.Stop();
            activePlayingRoutine.Stop();
            OnPlaybackFinish.Invoke();
            Debug.Log("invoed OnPlaybackFinish");
        }
    }

    IEnumerator Play(float delay)
    {
        player.Prepare();
        yield return new WaitForSeconds(delay);
        player.Play();
        yield return new WaitForEndOfFrame();

        while (player.isPlaying)
        {

            yield return null;
        }
        OnPlaybackFinish.Invoke();
    }
}
