﻿using System;
using System.Collections.Generic;
using Broken.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Scripts.MainGame
{
    public abstract class AttackHandler
    {
        private uint ID;

        public List<BoundingCircle> _colliders = new();

        protected List<uint> _attacked = new();
        protected float dmg = 10;
        protected Vector2 knockbackVelocity = new Vector2(0, 1);
        protected float knockbackSpeed = 200;
        protected float knockbackTime = 1;

        public AttackHandler(uint id)
        {
            ID = id;
        }
        public virtual void Attack(Entity attacker, EntityList entities)
        {
            foreach(Entity entity in entities)
            {
                if (entity.ID == ID) continue;
                if(_attacked.Contains(entity.ID)) continue;
                CheckDamageCollisions(entity);
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

        private void CheckDamageCollisions(Entity entity)
        {
            foreach (var col in _colliders)
            { 
                if (col.CollidesWith(entity.FloorCollider))
                {
                    entity.Damage(dmg, knockbackVelocity, knockbackSpeed, knockbackTime);
                    _attacked.Add(entity.ID);
                    break;
                }
            }
        }


    }
}
