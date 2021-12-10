using System.Threading.Tasks;
using CodeBase.Infrastructure.Services.IAP;

namespace CodeBase.Infrastructure.States
{
    public class LoadIAPState : IState
    {
        private GameStateMachine _gameStateMachine;
        private IIAPService _iapService;

        public LoadIAPState(GameStateMachine gameStateMachine, IIAPService iapService)
        {
            _gameStateMachine = gameStateMachine;
            _iapService = iapService;
        }

        public async void Enter()
        {
            await LoadIAP();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async Task LoadIAP()
        {
            await _iapService.Initialize();
        }

        public void Exit()
        {
        }
    }
}