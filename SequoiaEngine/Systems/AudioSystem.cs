using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using SequoiaEngine.Utilities;

namespace SequoiaEngine
{
    public class AudioSystem : System
    {
        public AudioSystem(SystemManager systemManager) : base(systemManager, typeof(AudioSource))
        { }

        protected override void Update(GameTime gameTime)
        {
            foreach (uint id in gameObjects.Keys)
            {
                AudioSource audioSource = gameObjects[id].GetComponent<AudioSource>();

                while (audioSource.soundEffectsQueue.Count > 0)
                {
                    SoundEffectInQueue effectName = audioSource.soundEffectsQueue.Dequeue();
                    SoundEffect effect = ResourceManager.Get<SoundEffect>(effectName.SoundEffectName);

                    effect.Play(effectName.VolumeLevel, effectName.Pitch, effectName.Pan);
                }

                if (audioSource.previousState == State.Stopped && audioSource.currentState == State.Playing)
                {
                    MediaPlayer.Play(ResourceManager.Get<Song>(audioSource.currentSong));
                    MediaPlayer.IsRepeating = audioSource.repeat;
                }
                else if (audioSource.previousState == State.Paused && audioSource.currentState == State.Playing)
                {
                    MediaPlayer.Resume();
                }
                else if (audioSource.previousState == State.Playing && audioSource.currentState == State.Paused)
                {
                    MediaPlayer.Pause();
                }
                else if (audioSource.previousState == State.Playing && audioSource.currentState == State.Stopped)
                {
                    MediaPlayer.Stop();
                }

                audioSource.previousState = audioSource.currentState;
                audioSource.previousSong = audioSource.currentSong;
            }
        }
    }
}