﻿using System.Threading.Tasks;

namespace _Client
{
    public class OneFrameSystem<T> : IInitSystem, IRunSystem where T : IComponent
    {
        private EcsFilter _filter;
        
        public async Task Init(EcsSystems systems)
        {
            _filter = systems
                .GetWorld()
                .Filter()
                .With<T>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                entity.RemoveComponent<T>();
            }
        }
    }
}