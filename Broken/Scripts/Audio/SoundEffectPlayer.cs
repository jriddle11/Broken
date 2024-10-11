using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Audio
{
    public static class SoundEffectPlayer
    {
        static SoundLibrary _library = new();
        static Game _game;

        public static void AddSound(string key, string soundPath)
        {
            _library.Add(key, soundPath, _game);
        }

        public static void Initialize(Game game)
        {
            _game = game;
            AddSound("SwordSwing", "External Assets/SoundEffects/SwordSwing");
            AddSound("SwordDmg", "External Assets/SoundEffects/SwordDmg");
            AddSound("FireLightUp", "External Assets/SoundEffects/FireLightUp");
        }

        public static void PlaySound(string key, float volume = 1f, float pan = 0f)
        {
            SoundEffectInstance instance = _library[key].CreateInstance();
            instance.Volume = volume;
            instance.Pan = pan;
            instance.Play();
        }
    }
}
