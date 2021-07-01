using System;
using Godot;
using GodotOnReady.Attributes;

namespace DodgetheCreeps2Dstarterproject.Hud
{
	public partial class HUD : CanvasLayer
	{
		[OnReadyGet]
		private Button _button;
		
		[OnReadyGet]
		private Label _scoreLabel;
		
		[OnReadyGet]
		private Label _messageLabel;
		
		[OnReadyGet]
		private Timer _timer;
	
		private int _score = 0;
		private string _message = string.Empty;

		[Export]
		public int Score
		{
			get => _score;
			set
			{
				if (value >= 0)
				{
					_score = value;
					_scoreLabel.Text = _score.ToString();
				}
			}
		}

		[Export()]
		public string Message
		{
			get => _message;
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					_message = value;
					_messageLabel.Text = _message;
					_messageLabel.Show();
				}
			}
		}

		[Signal]
		delegate void StartGame();

		public void ShowTemporaryMessage(string message)
		{
			Message = message;
			_timer.Start();
		}

		public async void ShowGameOver()
		{
			ShowTemporaryMessage("Game Over");
			await ToSignal(_timer, "timeout");
			Message = "Dodge the creeps";
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			Score = 0;
			_button.Show();
		}

		private void _OnButtonPressed()
		{
			_button.Hide();
			EmitSignal(nameof(StartGame));
		}
	
		private void _OnTimerTimeout()
		{
			Message = string.Empty;
			_messageLabel.Hide();
		}
	}
}

