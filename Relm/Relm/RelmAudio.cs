using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Relm
{
    public static class RelmAudio
    {
        private static Song _currentSong;
        
        public static void PlaySong(string songName, bool isRepeating = false)
        {
            _currentSong = RelmGame.Content.LoadSong(songName);
            MediaPlayer.Play(_currentSong);
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.MediaStateChanged += MediaPlayerOnMediaStateChanged;
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }

        private static void MediaPlayerOnMediaStateChanged(object sender, EventArgs e)
        {
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(_currentSong);
        }

        public static SoundEffectInstance PlaySoundEffect(string sfxName, bool isRepeating = false)
        {
            var soundEffect = RelmGame.Content.LoadSoundEffect(sfxName);
            var ret = soundEffect.CreateInstance();
            ret.IsLooped = isRepeating;
            ret.Play();
            return ret;
        }
    }
}