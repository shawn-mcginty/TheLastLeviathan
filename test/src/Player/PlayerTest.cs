namespace TheLastLeviathan.Player;

using System.Collections.Generic;
using Chickensoft.GoDotTest;
using Godot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public class PlayerTest : TestClass {

	public PlayerTest(Node n) : base(n) { }

	[Test]
	public void Test_Static_Player_ResolveInputs() {
		var left = new List<Player.MovementInputs> {
			Player.MovementInputs.MoveLeft,
		};
		var leftUp = new List<Player.MovementInputs> {
			Player.MovementInputs.MoveLeft,
			Player.MovementInputs.MoveUp,
		};
		var up = new List<Player.MovementInputs> {
			Player.MovementInputs.MoveUp,
		};

		//var shouldBeLeft = Player.ResolveInputs(left, leftUp);
		var shouldBeUp = Player.ResolveInputs(leftUp, up);

		//Assert.AreEqual(shouldBeLeft, Player.MovementInputs.MoveLeft);
		Assert.AreEqual(shouldBeUp, Player.MovementInputs.MoveUp);
	}

	[Test]
	public void Test_Static_Player_ResolveInputVector() {
		Assert.AreEqual(Player.ResolveInputVector(Player.MovementInputs.MoveLeft), Vector2.Left);
		Assert.AreEqual(Player.ResolveInputVector(Player.MovementInputs.MoveRight), Vector2.Right);
		Assert.AreEqual(Player.ResolveInputVector(Player.MovementInputs.MoveUp), Vector2.Up);
		Assert.AreEqual(Player.ResolveInputVector(Player.MovementInputs.MoveDown), Vector2.Down);
		Assert.AreEqual(Player.ResolveInputVector(null), Vector2.Zero);
	}
}
