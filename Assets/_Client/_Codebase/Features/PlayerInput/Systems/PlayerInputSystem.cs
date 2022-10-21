using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class PlayerInputSystem : IInitSystem, IRunSystem
    {
        private readonly InputChannel inputChannel = new ();

        private EcsFilter playerInputs;
        
        public async Task Init(EcsSystems systems)
        {
            playerInputs = systems
                .GetWorld()
                .Filter()
                .With<PlayerInput>().Build();
            
            inputChannel.Enable();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in playerInputs)
            {
                var input = entity.GetComponent<PlayerInput>();

                input.IsShootingSecondaryGun = inputChannel.Player.ShootSecondaryGun.WasPressedThisFrame();
                input.IsShootingPrimaryGun = inputChannel.Player.ShootPrimaryGun.WasPressedThisFrame();
                input.IsMoving = inputChannel.Player.Movement.IsPressed();
                input.Rotation = inputChannel.Player.Rotation.ReadValue<float>();
            }
        }
    }
}