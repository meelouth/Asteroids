namespace _Client
{
    public class PlayerInput : IComponent
    {
        public bool IsShootingPrimaryGun;
        public bool IsShootingSecondaryGun;
        public bool IsMoving;
        public float Rotation;
    }
}