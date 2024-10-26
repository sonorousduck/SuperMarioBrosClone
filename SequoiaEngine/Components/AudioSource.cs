using System;
using System.Collections.Generic;

namespace SequoiaEngine
{
    public enum State
    {
        Paused,
        Playing,
        Stopped
    }


    public class SoundEffectInQueue
    {
        public SoundEffectInQueue(string soundEffectName, float volumeLevel = 1.0f, float pitch = 0.0f, float pan = 0.0f) 
        {
            this.SoundEffectName = soundEffectName;
            this.VolumeLevel = volumeLevel;
            this.Pitch = pitch;
            this.Pan = pan;
        }

        public string SoundEffectName;
        public float VolumeLevel;
        public float Pitch;
        public float Pan;

    }
    public class AudioSource : Component
    {
        public State previousState;
        public State currentState;
        public string previousSong;
        public string currentSong;
        public bool repeat;
        public Queue<SoundEffectInQueue> soundEffectsQueue;

        public AudioSource()
        {
            currentState = State.Stopped;
            previousState = State.Stopped;
            currentSong = "";
            previousSong = "";
            repeat = false;
            soundEffectsQueue = new Queue<SoundEffectInQueue>();
        }
    }
}