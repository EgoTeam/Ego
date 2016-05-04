using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundtrackManager : MonoBehaviour {

    [SerializeField] public AudioSource[] _audioSources;
    public int index = 0;
    [SerializeField]
    public List<AudioSource> TrackQueue;

    void Start()
    {
        _audioSources = GetComponentsInChildren<AudioSource>();
        AddNextToQueue();
        StartCoroutine(PlayQueue());
    }
    public void AddNextToQueue()
    {
        if(index == _audioSources.Length)
        {
            index = 0;
        }
        TrackQueue.Add(_audioSources[index]);
        index += 1;
    }
    private IEnumerator PlayQueue()
    {
        while (TrackQueue.Count > 0)
        {
            TrackQueue[0].Play();
            while (TrackQueue.Count == 1)
            {
                TrackQueue[0].loop = true;
                yield return null;
            }
            TrackQueue[0].loop = false;
            while (TrackQueue[0].isPlaying)
            {
                yield return null;
            }

            TrackQueue.RemoveAt(0);
        }
    }
   /* public void PlayNext()
    {
        StartCoroutine(PlayNextCoroutine());
    }
    private IEnumerator PlayNextCoroutine()
    {
        if(_audioSources[index].loop == true)
        {
            _audioSources[index].loop = false;
        }
        while(_audioSources[index].isPlaying)
        {
            yield return null;
        }
        index += 1;
        if(index == _audioSources.Length)
        {
            index = 0;
        }
        _audioSources[index].Play();
    }*/

}
