using System.Threading.Tasks;

namespace _Client
{
    public interface IState
    {
        public Task Enter();
        public void Run();
        public void Exit();
    }
}