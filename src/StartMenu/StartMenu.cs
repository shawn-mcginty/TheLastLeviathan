namespace TheLastLeviathan;

using Chickensoft.GodotNodeInterfaces;
using Chickensoft.PowerUps;
using Godot;
using SuperNodes.Types;

public interface IStartMenu : IControl {
	event StartMenu.StartEventHandler Start;
}

[SuperNode(typeof(AutoNode))]
public partial class StartMenu : Control, IStartMenu {
	public override partial void _Notification(int what); // dunno, this is for chickensoft magic

	#region Nodes
	[Node]
	public IButton StartButton { get; set; } = default!;
	#endregion

	#region Signals
	[Signal]
	public delegate void StartEventHandler();
	#endregion

	public void OnReady() {
		GD.Print("StartMenu.OnReady");
		StartButton.Pressed += OnStartPressed;
	}

	public void OnExitTree() => StartButton.Pressed -= OnStartPressed;

	public void OnStartPressed() {
		GD.Print("StartMenu.OnStartPressed");
		EmitSignal(SignalName.Start);
	}
}
