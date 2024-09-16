using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using Broken.Scripts.Models;
using System.Collections.Generic;

namespace Broken.Scripts.MainGame
{
    /// <summary>
    /// Room object to be part of a collection making up a level in the dungeon
    /// </summary>
    public class Room : IGameObject
    {
        public Vector2 Bounds = new Vector2(1000, 1000);
        public List<BoundingRectangle> WallColliders { get; private set; } = new List<BoundingRectangle>();

        int _roomConfigID;
        RoomConfig _roomConfig;
        Texture2D _roomBackground;
        Texture2D _roomForeground;
        List<Texture2D> _roomOverlaps = new();
        List<IGameObject> _staticObjects = new();

        Texture2D _pixel;
        Color _pixColor = Color.White * .1f;

        public Room(Game game, int roomID)
        {
            _roomConfigID = roomID;
            LoadContent(game);
        }

        public void LoadContent(Game game)
        {
            _pixel = new Texture2D(OutputManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _roomConfig = FileManager.GetRoomConfigurationById(_roomConfigID);
            _roomBackground = game.Content.Load<Texture2D>(_roomConfig.Background);
            _roomForeground = game.Content.Load<Texture2D>(_roomConfig.Foreground);
            foreach (var overlap in _roomConfig.Overlaps) { _roomOverlaps.Add(game.Content.Load<Texture2D>(overlap)); }
            foreach (var rect in _roomConfig.WallColliders) { WallColliders.Add(new BoundingRectangle(rect)); }
            foreach(var loc in _roomConfig.TorchLocations) { _staticObjects.Add(new TorchFire(loc.ConvertToVector(), game)); }
            Bounds = new Vector2(_roomBackground.Width, _roomBackground.Height);

        }

        public void Update(GameTime gameTime)
        {
            foreach(var obj in _staticObjects){
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            spriteBatch.Draw(_roomBackground, Vector2.Zero, null, Color.LightBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            foreach(var overlap in _roomOverlaps)spriteBatch.Draw(overlap, Vector2.Zero, null, Color.LightBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);
            spriteBatch.Draw(_roomForeground, Vector2.Zero, null, Color.LightBlue * .6f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            foreach(var obj in _staticObjects) { obj.Draw(spriteBatch); }

            //spriteBatch.Draw(_pixel, new Rectangle(0, 0, 350, 1280), _pixColor);
        }
    }
}
