using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Broken.UI
{
    public class TitleCardSequence : IGameObject
    {
        public bool IsPlaying { get; private set; }
        Queue<TitleCard> _queue = new();
        TitleCard _currentCard;

        public bool IsDone => _queue.Count == 0 && (_currentCard == null || _currentCard.IsDone);

        public void Add(TitleCard card)
        {
            _queue.Enqueue(card);
        }

        public void Play()
        {
            if (_queue.Count > 0 && _currentCard == null)
            {
                IsPlaying = true;
                _currentCard = _queue.Dequeue();
                _currentCard.Display();
            }
        }

        public void LoadContent(Game game)
        {
            foreach (var card in _queue)
                card.LoadContent(game);

            _currentCard?.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            // Update current card
            if (_currentCard != null)
            {
                _currentCard.Update(gameTime);

                // If the current card is done, move to the next one in the queue
                if (_currentCard.IsDone)
                {
                    if (_queue.Count > 0)
                    {
                        _currentCard = _queue.Dequeue();
                        _currentCard.Display(); 
                    }
                    else
                    {
                        _currentCard = null;
                        IsPlaying = false; // Sequence is finished
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            // Draw the current card if there's one
            if (_currentCard != null && _currentCard.IsActive)
            {
                _currentCard.Draw(spriteBatch, opacity);
            }
        }
    }
}
