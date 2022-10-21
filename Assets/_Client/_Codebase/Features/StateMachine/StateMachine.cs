namespace _Client
{
    public class StateMachine
    {
        private IState _state;

        public async void SwitchState(IState newState)
        {
            _state?.Exit();

            _state = newState;
            
            await _state.Enter();
        }
        
        public void Run()
        {
            _state?.Run();
        }

        public void Destroy()
        {
            _state?.Exit();
        }
    }
}