using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Broken.Audio
{
    public class SoundLibrary : IEnumerable<SoundEffect>
    {
        private Dictionary<string, SoundEffect> _sounds = new();

        public SoundEffect this[string key]
        {
            get
            {
                if (_sounds.TryGetValue(key, out var sound))
                {
                    return sound;
                }
                throw new KeyNotFoundException($"Sound effect with key '{key}' not found.");
            }
            set
            {
                _sounds[key] = value;
            }
        }

        public void Add(string key, string path, Game game)
        {
            if(_sounds.TryGetValue(key, out var sound)) return;
            SoundEffect soundEffect = game.Content.Load<SoundEffect>(path);
            _sounds[key] = soundEffect;
        }

        public IEnumerator<SoundEffect> GetEnumerator()
        {
            return _sounds.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
