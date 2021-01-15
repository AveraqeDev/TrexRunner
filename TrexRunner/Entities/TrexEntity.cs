using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public class TrexEntity : IGameEntity
    {
        private const float GRAVITY = 500f;
        private const float JUMP_START_VELOCITY = -200f;

        private const int TREX_IDLE_BACKGROUND_SPRITE_POS_X = 40;
        private const int TREX_IDLE_BACKGROUND_SPRITE_POS_Y = 0;

        private const int TREX_DEFAULT_SPRITE_POS_X = 848;
        private const int TREX_DEFAULT_SPRITE_POS_Y = 0;
        private const int TREX_DEFAULT_SPRITE_WIDTH = 44;
        private const int TREX_DEFAULT_SPRITE_HEIGHT = 52;

        private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
        private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
        private const float BLINK_ANIMATION_DURATION = .5f;

        private Sprite _idleBackgroundSprite;
        private Sprite _idleSprite;
        private Sprite _idleBlinkSprite;

        private SoundEffect _jumpSound;

        private SpriteAnimation _blinkAnimation;

        private Random _random;

        private float _verticalVelocity;

        public Vector2 Position { get; set; }
        public TrexState State { get; private set; }
        public bool IsAlive { get; private set; }
        public float Speed { get; private set; }

        public int DrawOrder { get; set; }

        public TrexEntity(Texture2D spriteSheet, int positionX, int positionY, SoundEffect jumpSound)
        {
            Position = new Vector2(positionX, positionY - TREX_DEFAULT_SPRITE_HEIGHT);
            _idleBackgroundSprite = new Sprite(spriteSheet, TREX_IDLE_BACKGROUND_SPRITE_POS_X, TREX_IDLE_BACKGROUND_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            State = TrexState.Idle;

            _jumpSound = jumpSound;

            _random = new Random();

            _idleSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            _idleBlinkSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);

            _blinkAnimation = new SpriteAnimation();
            CreateBlinkAnimation();
        }

        public void Update(GameTime gameTime)
        {
            if(State == TrexState.Idle)
            {
                if (!_blinkAnimation.IsPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();

                }

                _blinkAnimation.Update(gameTime);
            }
            else if(State == TrexState.Jumping || State == TrexState.Falling)
            {
                Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds);
                _verticalVelocity += GRAVITY * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (State == TrexState.Idle)
            {
                _idleBackgroundSprite.Draw(spriteBatch, Position);
                _blinkAnimation.Draw(spriteBatch, Position);
            }
            else if (State == TrexState.Jumping || State == TrexState.Falling)
            {
                _idleSprite.Draw(spriteBatch, Position);
            }
        }

        private void CreateBlinkAnimation()
        {
            _blinkAnimation.Clear();
            _blinkAnimation.ShouldLoop = false;

            double _blinkTimeStamp = BLINK_ANIMATION_RANDOM_MIN + _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN);

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlinkSprite, (float) _blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, (float) _blinkTimeStamp + BLINK_ANIMATION_DURATION);
        }

        public bool StartJump()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;

            _jumpSound.Play();

            State = TrexState.Jumping;

            _verticalVelocity = JUMP_START_VELOCITY;

            return true;
        }

        public bool ContinueJump()
        {
            return true;
        }
    }
}
