using Godot;
using GodotOnReady.Attributes;

namespace DodgetheCreeps2Dstarterproject.Mob
{
	public partial class Mob : RigidBody2D
	{
		private float _minSpeed = 150.0f;
		private float _maxSpeed = 250.0f;
	
		[OnReadyGet()]
		private AnimatedSprite _animatedSprite;
	
		[Export]
		public float MinSpeed
		{
			get => _minSpeed;
			set
			{
				if (value > 0 && value < _maxSpeed)
				{
					_minSpeed = value;
				}
			}
		}

		[Export]
		public float MaxSpeed
		{
			get => _maxSpeed;
			set
			{
				if (value > 0 && value > _minSpeed)
				{
					_maxSpeed = value;
				}
			}
		}

		[OnReady]
		public void OnReady()
		{
			_animatedSprite.Playing = true;
			var mobTypes = _animatedSprite.Frames.GetAnimationNames();
			_animatedSprite.Animation = mobTypes[GD.Randi() % (mobTypes.Length -1)];
		}

		private static class Animation
		{
			public const string Name = "AnimatedSprite";
		}
	
		private void _OnVisibilityNotifier2DScreenExited()
		{
			QueueFree();
		}
	}
}


