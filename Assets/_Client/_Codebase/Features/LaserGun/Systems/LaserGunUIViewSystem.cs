using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Client
{
    public class LaserGunUIViewSystem : IInitSystem, IRunSystem
    {
        private readonly UIService _ui;
        
        private EcsFilter _laserGunsWithUI;

        public LaserGunUIViewSystem(UIService ui)
        {
            _ui = ui;
        }

        public async Task Init(EcsSystems systems)
        {
            _laserGunsWithUI = systems
                .GetWorld()
                .Filter()
                .With<LaserGun>().With<LaserGunUIViewRef>().Build();

            await CreateLaserUIView(systems);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _laserGunsWithUI)
            {
                var laserGun = entity.GetComponent<LaserGun>();
                var laserGunView = entity.GetComponent<LaserGunUIViewRef>().Ref;

                var fillAmount = GetLaserGunRemainingTimeToRecharge(laserGun);
                laserGunView.SetFill(fillAmount);
                laserGunView.SetValue(laserGun.LeftCharges);
            }
        }

        private static float GetLaserGunRemainingTimeToRecharge(LaserGun laserGun)
        {
            const float tolerance = 0.01f;
            
            return Math.Abs(laserGun.RemainingTimeToRecharge - laserGun.CooldownToRecharge) < tolerance
                ? 1
                : 1 - laserGun.RemainingTimeToRecharge / laserGun.CooldownToRecharge;
        }

        private async Task CreateLaserUIView(EcsSystems systems)
        {
            var attachCommands = systems
                .GetWorld()
                .Filter()
                .With<LaserGun>().With<AttachLaserGunUIViewCommand>().Build();

            foreach (var entity in attachCommands)
            {
                var command = entity.GetComponent<AttachLaserGunUIViewCommand>();

                var handle = Addressables.InstantiateAsync(command.AssetId);

                await handle.Task;

                var laserGunUIView = entity.AddComponent<LaserGunUIViewRef>();
                laserGunUIView.Ref = handle.Result.GetComponent<LaserGunUIView>();

                _ui.SecondaryWeaponWidget.Show();
                _ui.SecondaryWeaponWidget.AttachToRoot(laserGunUIView.Ref.transform);
            }
        }
    }
}