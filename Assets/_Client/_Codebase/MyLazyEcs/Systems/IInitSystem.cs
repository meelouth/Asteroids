using System.Threading.Tasks;

public interface IInitSystem : ISystem
{
    Task Init(EcsSystems systems);
}