using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace minicustomtowers
{
    public class SpriteBuilder
    {
        private static readonly Dictionary<Texture2D, Sprite> cache = new ();

        public static Sprite createProjectile(Texture2D tx)
        {
            if (!cache.ContainsKey(tx))
                cache.Add(tx, Sprite.Create(tx, new(0, 0, tx.width, tx.height), new(0.5f, 0.5f), 5.4f, 0, SpriteMeshType.Tight));
            return cache[tx];
        }
    }
}
