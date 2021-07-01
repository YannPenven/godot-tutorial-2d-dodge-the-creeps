using Godot;
using GodotOnReady.Attributes;

namespace DodgetheCreeps2Dstarterproject.Player
{
	public partial class Player : Area2D
	{
		private float _speed = 400.0f;
		private Vector2 _screenSize = Vector2.Zero;
	
		[OnReadyGet]
		private AnimatedSprite _animatedSprite;
	
		[OnReadyGet]
		private CollisionShape2D _collisionShape2D;

		[Export(PropertyHint.Range, "1,or_greater")]
		public float Speed
		{
			get => _speed;
			set
			{
				if (value > 0)
				{
					_speed = value;
				}
			}
		}

		[Signal]
		delegate void Hit();

		[OnReady]
		public void OnReady()
		{
			_screenSize = GetViewportRect().Size;
		}

		public void Start(Vector2 newPosition)
		{
			Position = newPosition;
			Show();
			_collisionShape2D.Disabled = false;
		}

		public override void _Process(float delta)
		{
			var direction = Vector2.Zero;
		
			if (Input.IsActionPressed(Constant.Movement.MoveRight))
			{
				direction.x += 1;
			}
		
			if (Input.IsActionPressed(Constant.Movement.MoveLeft))
			{
				direction.x -= 1;
			}
		
			if (Input.IsActionPressed(Constant.Movement.MoveDown))
			{
				direction.y += 1;
			}
		
			if (Input.IsActionPressed(Constant.Movement.MoveUp))
			{
				direction.y -= 1;
			}

			if (direction.Length() > 0)
			{
				direction = direction.Normalized();
				_animatedSprite.Play();
			}
			else
			{
				_animatedSprite.Stop();
			}

			Position += direction * _speed * delta;
			Position = new Vector2(Mathf.Clamp(Position.x, 0, _screenSize.x), Mathf.Clamp(Position.y, 0, _screenSize.y));

			if (direction.x != 0)
			{
				_animatedSprite.Animation = Animation.Right;
				_animatedSprite.FlipV = false;
				_animatedSprite.FlipH = direction.x < 0;
			}
			else if (direction.y != 0)
			{
				_animatedSprite.Animation = Animation.Up;
				_animatedSprite.FlipH = false;
				_animatedSprite.FlipV = direction.y > 0;
			}
		}
	
		private void _OnPlayerBodyEntered(object body)
		{
			Hide();
			_collisionShape2D.SetDeferred(nameof(CollisionShape2D.Disabled), value: true);
			EmitSignal(nameof(Hit));
		}
	
		private static class Animation
		{
			public const string Name = "AnimatedSprite";
			public const string Right = "right";
			public const string Up = "up";
		}
	}
}


