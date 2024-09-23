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
        public bool TimesUp => _currentTime > _setTime;

        private double _setTime;
        private double _currentTime;

        public Timer(double setTime)
        {
            _setTime = setTime;
            _currentTime = 0;
        }

        public bool TimeIsUp(GameTime gametime)
        {
            Update(gametime);
            return TimesUp;
        }

        public void Update(GameTime gametime)
        {
            _currentTime += gametime.ElapsedGameTime.TotalSeconds;
        }

        public void Reset()
        {
            _currentTime = 0;
        }

        public void ChangeSetTime(float time)
        {
            _setTime = time;
        }
    }
}
