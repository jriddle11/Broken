using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Broken.Entities
{
    public abstract class AttackHandler
    {
        private uint ID;

        public List<BoundingCircle> _colliders = new();

        protected List<uint> _attacked = new();
        protected Vector2 knockbackVelocity = new Vector2(0, 1);
        protected float knockbackSpeed = 200;
        protected float knockbackTime = 1;

        public AttackHandler(uint id)
        {
            ID = id;
        }
        public virtual void Attack(Character attacker, CharacterList entities, out bool attackHit)
        {
            attackHit = false;
            foreach(Character entity in entities)
            {
                if (entity.ID == ID) continue;
                if(_attacked.Contains(entity.ID)) continue;
                CheckDamageCollisions(entity, out bool hit);
                if(hit) attackHit = true;
            }
            _attacked.Clear();
        }

        public void DrawColliders(SpriteBatch sprite)
        {
            foreach (var collider in _colliders)
            {
                DevManager.DrawCircle(sprite, collider, Color.LightBlue * 0.5f);
            }
        }

        private void CheckDamageCollisions(Character entity, out bool hit)
        {
            hit = false;
            foreach (var col in _colliders)
            { 
                if (col.CollidesWith(entity.Collider))
                {
                    hit = true;
                    entity.Damage(1, knockbackVelocity, knockbackSpeed, knockbackTime);
                    _attacked.Add(entity.ID);
                    break;
                }
            }
        }
    }
}
