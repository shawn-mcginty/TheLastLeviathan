namespace TheLastLeviathan.PlayerCamera;

using Chickensoft.GodotNodeInterfaces;
using Chickensoft.PowerUps;
using Godot;
using SuperNodes.Types;

public interface IPlayerCamera : ICamera2D {
	void UsePLayerCamera();
}

[SuperNode(typeof(AutoNode))]
public partial class PlayerCamera : Camera2D, IPlayerCamera {
	public override partial void _Notification(int what); // dunno, this is for chickensoft magic

	public void UsePLayerCamera() => MakeCurrent();
}
