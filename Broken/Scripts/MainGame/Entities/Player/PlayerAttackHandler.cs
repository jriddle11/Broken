using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broken.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Scripts.MainGame
{
    public class PlayerAttackHandler : AttackHandler
    {
        public PlayerAttackHandler(uint id) : base(id)
        {
            _colliders.Add(new BoundingCircle(new Vector2(1000,1000), 180));
            _colliders.Add(new BoundingCircle(new Vector2(1000, 1000), 70));
            _colliders.Add(new BoundingCircle(new Vector2(1000, 1000), 70));
        }

        public override void Attack(Entity attacker, EntityList entities)
        {
            var player = attacker as Player;
            SetupDamageColliders(player);
            SetupAttackModifiers(player);
            base.Attack(attacker, entities);
        }

        private void SetupDamageColliders(Player player)
        {
            var pos = player.CenterPosition + new Vector2(0, 0);
            var col = _colliders[0];
            col.Center = (pos + (DirectionHelper.GetDirectionalVelocity(Vector2.Zero, player.Direction) * player.SwordReach * 1.2f));
            _colliders[0] = col;
            col = _colliders[1];
            col.Center = (pos + (DirectionHelper.GetDirectionalVelocity(Vector2.Zero, DirectionHelper.GetDirectionClockwise(player.Direction)) * player.SwordReach * 1.7f));
            _colliders[1] = col;
            col = _colliders[2];
            col.Center = (pos + (DirectionHelper.GetDirectionalVelocity(Vector2.Zero, DirectionHelper.GetDirectionCounterClockwise(player.Direction)) * player.SwordReach * 1.7f));
            _colliders[2] = col;
        }

        private void SetupAttackModifiers(Player player)
        {
            knockbackVelocity = DirectionHelper.GetDirectionalVelocity(Vector2.Zero, player.Direction);
            knockbackSpeed = 600f;
            knockbackTime = 0.2f;
        }
    }
}
