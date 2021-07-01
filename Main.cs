using DodgetheCreeps2Dstarterproject.Hud;
using Godot;
using GodotOnReady.Attributes;

namespace DodgetheCreeps2Dstarterproject
{
	public partial class Main : Node
	{
		[OnReadyGet()]
		private PathFollow2D _mobSpawnLocation;
	
		[OnReadyGet()]
		private Timer _scoreTimer;
	
		[OnReadyGet()]
		private Timer _startTimer;
	
		[OnReadyGet()]
		private Timer _mobTimer;
	
		[OnReadyGet()]
		private HUD _hud;
	
		[OnReadyGet()]
		private DodgetheCreeps2Dstarterproject.Player.Player _player;
	
		[OnReadyGet()]
		private Position2D _startPosition;
	
		[Export]
		public PackedScene MobScene;

		[OnReady]
		public void OnReady()
		{
			GD.Randomize();
		}

		public async void NewGame()
		{
			_hud.Score = 0;
			GetTree().CallGroup("mobs", "queue_free");
			_player.Start(_startPosition.Position);
			_startTimer.Start();
			_hud.ShowTemporaryMessage("Get Ready...");
			await ToSignal(_startTimer, "timeout");
			_scoreTimer.Start();
			_mobTimer.Start();
		}

		public void GameOver()
		{
			_scoreTimer.Stop();
			_mobTimer.Stop();
			_hud.ShowGameOver();
		}

		private void _OnMobTimerTimeout()
		{
			_mobSpawnLocation.UnitOffset = GD.Randf();
			var mob = (DodgetheCreeps2Dstarterproject.Mob.Mob)MobScene.Instance();
			AddChild(mob);
			mob.Position = _mobSpawnLocation.Position;
			var direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;
			direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
			mob.Rotation = direction;

			var velocity = new Vector2((float)GD.RandRange(mob.MinSpeed, mob.MaxSpeed), 0);
			mob.LinearVelocity = velocity.Rotated(direction);
		}
	
		private void _OnScoreTimerTimeout()
		{
			_hud.Score += 1;
		}
	}
}
