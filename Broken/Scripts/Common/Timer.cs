using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Scripts.Common
{
    /// <summary>
    /// Object for timing things
    /// </summary>
    public class Timer
    {
        public double TimeLeft => _setTime - _currentTime;
        public double TimePercentLeft => _currentTime / _setTime;

        private double _setTime;
        private double _currentTime;

        public Timer(double setTime)
        {
            _setTime = setTime;
            _currentTime = 0;
        }

        public bool TimeIsUp(GameTime gametime)
        {
            _currentTime += gametime.ElapsedGameTime.TotalSeconds;
            if(_currentTime > _setTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _currentTime = 0;
        }
    }
}
