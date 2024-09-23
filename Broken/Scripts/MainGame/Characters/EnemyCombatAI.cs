using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using System.Collections.Generic;
using System;
using Broken.Scripts.Models;

namespace Broken.Scripts.MainGame
{
    public class EnemyCombatAI
    {
        public EnemyCombatPersonality Personality;
        public float DecisionSpeed { get; private set; }
        public bool HasActedOnLastDecision = true;

        Timer _decisionTimer;
        AICombatDecision _lastDecision = AICombatDecision.DoNothing;

        public EnemyCombatAI(float decisionSpeed = 2f, EnemyCombatPersonality personality = EnemyCombatPersonality.Mild)
        {
            Personality = personality;
            DecisionSpeed = decisionSpeed;
            _decisionTimer = new Timer(DecisionSpeed);
        }

        public void Update(GameTime gameTime)
        {
            _decisionTimer.Update(gameTime);
        }

        public AICombatDecision GetNextDecision(EnemyState enemyState)
        {
            if (!enemyState.AwareOfPlayer)
            {
                return MakeIdleDecision();
            }
            else if (_decisionTimer.TimesUp && HasActedOnLastDecision)
            {
                HasActedOnLastDecision = false;
                return MakeCombatDecision();
            }
            else
            {
                return AICombatDecision.DoNothing;
            }
        }

        public void ActionComplete()
        {
            HasActedOnLastDecision = true;
            _decisionTimer.Reset();
        }

        private AICombatDecision MakeCombatDecision()
        {
            return AICombatDecision.MoveToPlayer;
        }

        private AICombatDecision MakeIdleDecision()
        {
            return _lastDecision;
        }
    }

    public enum EnemyCombatPersonality
    {
        Mild, Aggressive, Defensive, Guardian
    }

    public enum AICombatDecision
    {
        DoNothing, MoveToPlayer, Attack, Block, MoveAwayFromPlayer, MoveRandomly, GoBackToSpawn
    }
}
