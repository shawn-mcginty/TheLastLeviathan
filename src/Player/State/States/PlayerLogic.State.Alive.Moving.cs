namespace TheLastLeviathan.Player;

using Godot;

public partial class PlayerLogic {
	public abstract partial record State {
		public record Moving : Alive, IGet<Input.StoppedMoving>, IGet<Input.ChangeDirection> {
			public Moving(IContext context) : base(context) {
				OnEnter<Moving>(
					(previous) => {
						GD.Print("PlayerLogic.State.Moving.OnEnter");
						Context.Output(new Output.Animations.Walk());
					}
				);
			}

			public IState On(Input.StoppedMoving input) => new Idle(Context);

			public IState On(Input.ChangeDirection input) {
				GD.Print("PlayerLogic.State.Moving.On(Input.ChangeDirection)");
				Context.Output(new Output.Animations.Idle());
				Context.Output(new Output.Animations.Walk());
				return this;
			}
		}
	}
}
