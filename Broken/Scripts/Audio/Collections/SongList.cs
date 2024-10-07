using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Broken.Audio
{
    public class SongList : IEnumerable<Song>
    {
        private Dictionary<string, Song> _songs = new();

        public Song this[string key]
        {
            get
            {
                if (_songs.TryGetValue(key, out var song))
                {
                    return song;
                }
                throw new KeyNotFoundException($"Song with key '{key}' not found.");
            }
            set
            {
                _songs[key] = value;
            }
        }

        public void Add(string key, string path, Game game)
        {
            Song song = game.Content.Load<Song>(path);
            _songs[key] = song;
        }

        public IEnumerator<Song> GetEnumerator()
        {
            return _songs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
