namespace TheLastLeviathan.Player;

public partial class PlayerLogic {
	/// <summary>Player settings.</summary>
	/// <param name="Gravity">Gravity</param>
	/// <param name="MoveSpeed">Movement speed</param>
	/// <param name="Acceleration">Acceleration</param>
	public record Settings(
		float Gravity,
		float MoveSpeed,
		float Acceleration,
		float StoppingSpeed
	);
}
