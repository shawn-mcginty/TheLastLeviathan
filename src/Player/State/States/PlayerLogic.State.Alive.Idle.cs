namespace TheLastLeviathan.Player;

using Godot;

public partial class PlayerLogic {
	public abstract partial record State {
		public record Idle : Alive, IGet<Input.StartedMoving> {
			public Idle(IContext context) : base(context) {
				OnEnter<Idle>(
					(previous) => {
						GD.Print("PlayerLogic.State.Idle.OnEnter");
						Context.Output(new Output.Animations.Idle());
					}
				);
			}

			public IState On(Input.StartedMoving input) => new Moving(Context);
		}
	}
}
