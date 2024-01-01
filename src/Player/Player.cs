namespace TheLastLeviathan.Player;

using System.Collections.Generic;
using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.PowerUps;
using Godot;
using SuperNodes.Types;
using TheLastLeviathan.Game;

public interface IPlayer : ICharacterBody2D {
	public Vector2 GetGlobalInputVector();

	public List<Player.MovementInputs> GetMovementInputs();
}

[SuperNode(typeof(Provider), typeof(Dependent), typeof(AutoNode))]
public partial class Player : CharacterBody2D, IPlayer, IProvide<IPlayerLogic> {
	public enum MovementInputs {
		MoveLeft,
		MoveRight,
		MoveUp,
		MoveDown
	}

	public override partial void _Notification(int what); // dunno, this is for chickensoft magic

	#region Exports
	/// <summary>Player gravity (meters/sec).</summary>
	[Export(PropertyHint.Range, "-100, 0, 0.1")]
	public float Gravity { get; set; } = -30.0f;

	/// <summary>Player speed (meters/sec).</summary>
	[Export(PropertyHint.Range, "0, 100, 0.1")]
	public float MoveSpeed { get; set; } = 8f;

	/// <summary>Player speed (meters^2/sec).</summary>
	[Export(PropertyHint.Range, "0, 100, 0.1")]
	public float Acceleration { get; set; } = 4f;

	/// <summary>Stopping velocity (meters/sec).</summary>
	[Export(PropertyHint.Range, "0, 100, 0.1")]
	public float StoppingSpeed { get; set; } = 1.0f;
	#endregion

	#region Provisions
	IPlayerLogic IProvide<IPlayerLogic>.Value() => PlayerLogic;
	#endregion

	#region Dependencies
	[Dependency]
	public IAppRepo AppRepo => DependOn<IAppRepo>();
	[Dependency]
	public IGameRepo GameRepo => DependOn<IGameRepo>();
	#endregion

	#region State
	public IPlayerLogic PlayerLogic { get; set; } = default!;
	public PlayerLogic.IBinding PlayerBinding { get; set; } = default!;
	public PlayerLogic.Settings Settings { get; set; } = default!;
	#endregion

	#region Nodes
	[Node]
	public IAnimatedSprite2D CharacterSprite { get; set; } = default!;
	#endregion

	public void Setup() {
		Settings = new PlayerLogic.Settings(
			Gravity,
			MoveSpeed,
			Acceleration,
			StoppingSpeed
		);

		PlayerLogic = new PlayerLogic(
			player: this,
			settings: Settings,
			appRepo: AppRepo,
			gameRepo: GameRepo
		);
	}

	public void OnReady() => SetPhysicsProcess(true);

	public void OnExitTree() {
		PlayerLogic.Stop();
		PlayerBinding.Dispose();
	}

	public void OnResolved() {
		GD.Print("Player.OnResolved");
		Provide();

		PlayerBinding = PlayerLogic.Bind();

		GameRepo.SetPlayerGlobalPosition(GlobalPosition);

		PlayerBinding
			.Handle<PlayerLogic.Output.MovementComputed>(
				(output) => Velocity = output.Velocity)
			.Handle<PlayerLogic.Output.Animations.Idle>(
				(output) => {
					var data = PlayerLogic.Get<PlayerLogic.Data>();
					CharacterSprite.FlipH = false;

					if (data.LastStrongDirection == Vector2.Up) {
						CharacterSprite.Play("BackIdle");
					}
					else if (data.LastStrongDirection == Vector2.Left) {
						CharacterSprite.FlipH = true;
						CharacterSprite.Play("SideIdle");
					}
					else if (data.LastStrongDirection == Vector2.Right) {
						CharacterSprite.Play("SideIdle");
					}
					else {
						CharacterSprite.Play("FrontIdle");
					}
				})
			.Handle<PlayerLogic.Output.Animations.Walk>(
				(output) => {
					var data = PlayerLogic.Get<PlayerLogic.Data>();
					CharacterSprite.FlipH = false;

					if (data.StrongInput == MovementInputs.MoveUp) {
						CharacterSprite.Play("BackWalk");
					}
					else if (data.StrongInput == MovementInputs.MoveLeft) {
						CharacterSprite.FlipH = true;
						CharacterSprite.Play("SideWalk");
					}
					else if (data.StrongInput == MovementInputs.MoveRight) {
						CharacterSprite.Play("SideWalk");
					}
					else {
						CharacterSprite.Play("FrontWalk");
					}
				});

		PlayerLogic.Start();
	}
	public void OnPhysicsProcess(double delta) {
		PlayerLogic.Input(new PlayerLogic.Input.PhysicsTick(delta));
		MoveAndSlide();
		PlayerLogic.Input(new PlayerLogic.Input.Moved(GlobalPosition));
	}

	public Vector2 GetGlobalInputVector() {
		var rawInput = Input.GetVector(
			GameInputs.MoveLeft,
			GameInputs.MoveRight,
			GameInputs.MoveUp,
			GameInputs.MoveDown
		);

		var input = new Vector2(
			x: rawInput.X,
			y: rawInput.Y
		);

		return input;
	}

	/// <summary>
	/// Returns a list of movement inputs in order of priority.
	/// </summary>
	public List<MovementInputs> GetMovementInputs() {
		var inputs = new List<MovementInputs>();

		// input priority: the lower in this if chain, the higher the priority
		if (Input.IsActionPressed(GameInputs.MoveDown)) {
			inputs.Add(MovementInputs.MoveDown);
		}
		if (Input.IsActionPressed(GameInputs.MoveUp)) {
			inputs.Add(MovementInputs.MoveUp);
		}
		if (Input.IsActionPressed(GameInputs.MoveLeft)) {
			inputs.Add(MovementInputs.MoveLeft);
		}
		if (Input.IsActionPressed(GameInputs.MoveRight)) {
			inputs.Add(MovementInputs.MoveRight);
		}

		return inputs;
	}

	public static MovementInputs? ResolveInputs(List<MovementInputs> previous, List<MovementInputs> current) {
		if (current.Count == 0) {
			return null;
		}

		var newInputs = current.Except(previous).ToList();
		var sharedInputs = previous.Intersect(current).ToList();

		if (newInputs.Count == 0 && previous.Count == 0) {
			return null;
		}

		if (newInputs.Count == 0 && (previous.Count == current.Count)) {
			return previous.First();
		}

		if (sharedInputs.Count == 0) {
			return current.First();
		}

		return sharedInputs.First();
	}

	public static Vector2 ResolveInputVector(MovementInputs? input) => input switch {
		MovementInputs.MoveLeft => Vector2.Left,
		MovementInputs.MoveRight => Vector2.Right,
		MovementInputs.MoveUp => Vector2.Up,
		MovementInputs.MoveDown => Vector2.Down,
		_ => Vector2.Zero
	};
}
