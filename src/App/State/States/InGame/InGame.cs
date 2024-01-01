namespace TheLastLeviathan.App;


public partial class AppLogic {
	public partial record State {
		public record InGame : State, IGet<Input.GoToMainMenu>, IGet<Input.GameOver> {
			public InGame(IContext context) : base(context) {
				var appRepo = Context.Get<IAppRepo>();
				OnEnter<InGame>(
					(previous) => {
						appRepo.OnStartGame();
						Context.Output(new Output.ShowGame());
					}
				);
			}

			public IState On(Input.GameOver input) => new PlayingGame(Context);
			public IState On(Input.GoToMainMenu input) => new PlayingGame(Context);
		}
	}
}
