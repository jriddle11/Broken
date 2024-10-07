using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace Broken.Audio
{
    public static class MusicPlayer
    {
        public static bool IsPlaying => MediaPlayer.State == MediaState.Playing;
        static SongList _songs = new();

        static float _currentSetVolume;
        static bool _fading = false;
        static Timer _timer;

        public static void Initialize(BrokenGame game)
        {
            _songs.Add("WorthyAdversary", "External Assets/Music/mainMenuTheme", game);
            MediaPlayer.IsMuted = game.DebugMode;
        }

        public static void Play(string name, bool looping, float volume)
        {
            if (!IsPlaying)
            {
                _currentSetVolume = volume;

                MediaPlayer.Volume = volume;
                MediaPlayer.IsRepeating = looping;
                MediaPlayer.Play(_songs[name]);
            }

        }

        public static void Update(GameTime gameTime)
        {
            if (!_fading) return;
            if (_timer == null) return;
            _timer.Update(gameTime);
            MediaPlayer.Volume = _currentSetVolume * (1 - (float)_timer.TimePercentLeft);
            if (_timer.TimesUp)
            {
                Stop();
            }
        }

        public static void SlowStop(float time)
        {
            _timer = new Timer(time);
            _fading = true;
        }

        public static void Stop()
        {
            _fading = false;
            MediaPlayer.Stop();
        }

    }
}
