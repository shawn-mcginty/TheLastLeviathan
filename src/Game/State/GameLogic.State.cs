namespace TheLastLeviathan.Game;

using Godot;

public partial class GameLogic {
	public interface IState : IStateLogic { }

	public record State : StateLogic, IState, IGet<Input.Initialize> {
		public State(IContext context) : base(context) {
			var appRepo = Context.Get<IAppRepo>();
			OnEnter<State>(
				(previous) => {
					GD.Print("GameLogic.State.OnEnter");
					appRepo.GameStarting += GameAboutToStart;
					appRepo.GamePaused += GamePaused;
					appRepo.GameResumed += GameResumed;
				}
			);

			OnExit<State>(
				(next) => {
					GD.Print("GameLogic.State.OnExit");
					appRepo.GameStarting -= GameAboutToStart;
					appRepo.GamePaused -= GamePaused;
					appRepo.GameResumed -= GameResumed;
				}
			);
		}

		public void GameAboutToStart() => Context.Output(new Output.FocusPlayer());

		public void GamePaused() => Context.Output(new Output.SetPauseMode(true));

		public void GameResumed() => Context.Output(new Output.SetPauseMode(false));

		public IState On(Input.Initialize input) {
			GD.Print("GameLogic.State.OnInitialized");
			return this;
		}
	}
}
