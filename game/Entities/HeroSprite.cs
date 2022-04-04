using Godot;
using ldjam50.Interfaces;
using System;

namespace ldjam50.Entities
{
    public class OuchiesArgs : EventArgs {
        public OuchiesArgs(int health = 6) {
            Health = health;
        }

        public int Health = 6;
    }
    public class HeroSprite : Area2D, IMovable
    {
        public MovementComponent Movement => _Movement ??= GetNode<MovementComponent>("MovementComponent") ?? throw new Exception("No movement component on hero");
        private MovementComponent _Movement;

        public PlayerController Controller => _Controller ??= GetNode<PlayerController>("Controller") ?? throw new Exception("No controller found on hero");
        private PlayerController _Controller;
        public AudioStreamPlayer2D HitAudioPlayer => _HitAudioPlayer ??= GetNode<AudioStreamPlayer2D>("Hit1Player") ?? throw new Exception("No hit audio player on Hero");
        private AudioStreamPlayer2D _HitAudioPlayer;
        public AudioStreamPlayer2D ShootAudioPlayer => _ShootAudioPlayer ??= GetNode<AudioStreamPlayer2D>("PlayerShoot") ?? throw new Exception("No shoot audio player on Hero");
        private AudioStreamPlayer2D _ShootAudioPlayer;
        public AudioStreamPlayer2D HornAudioPlayer => _HornAudioPlayer ??= GetNode<AudioStreamPlayer2D>("HornPlayer") ?? throw new Exception("No horn audio player on Hero");
        private AudioStreamPlayer2D _HornAudioPlayer;
        const String BulletScenePath = "res://Bullet.tscn";
        public PackedScene BulletScene;

        public float ShootCooldown = 0.5f;
        public int Health = 6;
        private int MaxHealth = 6;
        private Boolean ShootAvailable = true;
        private Boolean HasHorn = false;

        public event EventHandler<OuchiesArgs> Ouchies;
        public event EventHandler<EventArgs> Ded;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Connect("area_entered", this, nameof(OnAreaEntered));
            //Rotation = Mathf.Pi * -0.5f;
        }

        public override void _Ready()
        {
            Movement.InterpSpeed = new(0.3f, 0.3f);
            BulletScene = GD.Load<PackedScene>(BulletScenePath);
        }

        /// <summary>
        /// Physics Process
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(Single delta)
        {
            var input = Controller.InputVector;
            var screenStart = GetParent<Node2D>().ToLocal(this.ScreenRect().Position);
            var screenEnd = GetParent<Node2D>().ToLocal(this.ScreenRect().End);
            var bounds = GetNode<Sprite>("Sprite").GetRect();
            var position = Position;

            if (input.x < 0f && position.x + bounds.Position.x < screenStart.x + Global.ScreenBuffer)
            {
                input.x = 0f;
                //position.x = (screenStart.x + Global.ScreenBuffer) - bounds.Position.x;
            }

            if (input.y < 0f && position.y + bounds.Position.y < screenStart.y + Global.ScreenBuffer)
            {
                input.y = 0f;
            }

            if (input.y > 0f && position.y + bounds.End.y > screenEnd.y - Global.ScreenBuffer)
            {
                input.y = 0f;
            }

            Movement.TargetDirection = input;

            if (Controller.Shoot && ShootAvailable) {
                // HornAudioPlayer.Play();
                (HasHorn ? HornAudioPlayer : ShootAudioPlayer).Play();
                ShootAvailable = false;
                var bullet = BulletScene.Instance<Bullet>();
                GetTree().Root.AddChild(bullet);
                bullet.GlobalPosition = GlobalPosition;
                bullet.FromPlayer = true;

                Global.Time.AddNotify(ShootCooldown, () => { ShootAvailable = true; });
            }
        }
        private void OnAreaEntered(Area2D other) {
            if (other.Name == "SecretThing") {
                HasHorn = true;
                return;
            }
            if (other is Bullet && ((Bullet)other).FromPlayer) {
                return;
            }
            //take damage
            HitAudioPlayer.Play();
            Health = Health - 1;
            if (Health == 0) {
                // Death
                Ded?.Invoke(this, new EventArgs());
            } else {
                Ouchies?.Invoke(this, new OuchiesArgs(Health));
            }
        }
    }
}
