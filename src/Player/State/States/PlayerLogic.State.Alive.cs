namespace TheLastLeviathan.Player;

using Godot;
using TheLastLeviathan.Game;

public partial class PlayerLogic {
	public abstract partial record State : StateLogic, IState {
		public record Alive : State, IGet<Input.PhysicsTick>, IGet<Input.Moved> {
			public Alive(IContext context) : base(context) {
				OnEnter<Idle>(
					(previous) => GD.Print("PlayerLogic.State.Alive.OnEnter"));
			}

			public IState On(Input.PhysicsTick input) {
				var delta = input.Delta;
				var player = Context.Get<IPlayer>();
				var gameRepo = Context.Get<IGameRepo>();
				var settings = Context.Get<Settings>();
				var data = Context.Get<Data>();
				var currentInputs = player.GetMovementInputs();
				var lastInputs = data.MovementInputs;
				var inputs = Player.ResolveInputs(lastInputs, currentInputs);
				var moveDirection = Player.ResolveInputVector(inputs);

				if (moveDirection.Length() > 0.2f) {
					data.LastStrongDirection = moveDirection.Normalized();
					GD.Print(data.LastStrongDirection);
				}

				data.LastVelocity = player.Velocity;
				data.LastMovementInputs = data.MovementInputs;
				data.MovementInputs = currentInputs;
				data.LastStrongInput = data.StrongInput;
				data.StrongInput = inputs;

				var velocity = data.LastVelocity with { Y = 0f };
				velocity = velocity.Lerp(
					moveDirection * settings.MoveSpeed,
					settings.Acceleration * (float)delta
				);

				if (moveDirection.Length() == 0f && velocity.Length() < settings.StoppingSpeed) {
					velocity = Vector2.Zero;
				}

				Context.Output(new Output.MovementComputed(velocity));

				return this;
			}
			public IState On(Input.Moved input) {
				var player = Context.Get<IPlayer>();
				var settings = Context.Get<Settings>();
				var gameRepo = Context.Get<IGameRepo>();
				var data = Context.Get<Data>();

				gameRepo.SetPlayerGlobalPosition(input.GlobalPosition);

				var speed = player.Velocity.Length();
				var lastSpeed = data.LastVelocity.Length();
				var lastInput = data.LastStrongInput;
				var currentInput = data.StrongInput;

				var justStartedMoving = lastInput == null && currentInput != null;
				var justStoppedMoving = lastInput != null && currentInput == null;
				var directionChanged = !justStartedMoving && !justStoppedMoving && lastInput != currentInput;
				if (directionChanged) {
					GD.PrintT("--", justStartedMoving, justStoppedMoving, directionChanged, data.LastVelocity.Normalized(), player.Velocity.Normalized(), lastSpeed, speed);
				}

				data.LastVelocity = player.Velocity;

				if (justStartedMoving) {
					Context.Input(new Input.StartedMoving());
				}
				else if (justStoppedMoving) {
					Context.Input(new Input.StoppedMoving());
				}
				else if (directionChanged && speed > settings.StoppingSpeed) {
					Context.Input(new Input.ChangeDirection());
				}

				return this;
			}
		}
	}
}
