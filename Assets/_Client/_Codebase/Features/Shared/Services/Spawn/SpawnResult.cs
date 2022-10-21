using UnityEngine;

namespace _Client
{
    public class SpawnResult
    {
        public Vector2 Position { get; }
        public Vector2 Direction { get; }

        public SpawnResult(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}