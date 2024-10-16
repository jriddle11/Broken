﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    /// <summary>
    /// Maintains a dictionary containing Texture2D objects
    /// </summary>
    public class TextureLibrary : IEnumerable<Texture2D>
    {
        private Dictionary<string, Texture2D> _textures = new();

        public Texture2D this[string key]
        {
            get
            {
                if (_textures.TryGetValue(key, out var texture))
                {
                    return texture;
                }
                throw new KeyNotFoundException($"Texture with key '{key}' not found.");
            }
            set
            {
                _textures[key] = value;
            }
        }

        public void Add(string key, string path, Game game)
        {
            Texture2D texture = game.Content.Load<Texture2D>(path);
            _textures[key] = texture;
        }

        public IEnumerator<Texture2D> GetEnumerator()
        {
            return _textures.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
