using UnityEngine;

namespace _Client
{
    public class SpawnService
    {
        public SpawnResult GenerateRandomPosition(float distance)
        {
            var spawnDirection = Random.insideUnitCircle.normalized;
            var spawnPoint = spawnDirection * distance;

            return new SpawnResult(spawnPoint, spawnDirection);
        }
    }
}