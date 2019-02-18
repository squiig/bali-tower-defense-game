using System.Collections;
using UnityEngine;
using Game.Audio;

/// <summary>
/// Used temporarily for debugging
/// </summary>
public class AudioBehaviourDebug : MonoBehaviour
{
    [SerializeField] private string identifier;

    void Start()
    {
        Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, "debug")); // Plays sound with indentifier 
    }
}
