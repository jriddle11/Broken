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
    
        public static void AddSound(string key, string soundPath, Game game)
        {
            _library.Add(key, soundPath, game);
        }

        public static void LoadContent(Game game)
        {
            AddSound("SwordSwing", "External Assets/SoundEffects/SwordSwing", game);
            AddSound("SwordDmg", "External Assets/SoundEffects/SwordDmg", game);
            AddSound("FireLightUp", "External Assets/SoundEffects/FireLightUp", game);
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
