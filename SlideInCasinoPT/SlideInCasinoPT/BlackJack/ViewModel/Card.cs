using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.ViewModel
{
    public class Card : CCNode
    {

        public CCTexture2D FrontTexture, BackTexture;
        public CCRect FrontTextureRect, BackTextureRect;


        public CCRenderTexture RenderTexture;

        private Random random;

        public override byte Opacity
        {
            get { return base.Opacity; }

            set
            {
                base.Opacity = value;
                this.RenderTexture.Sprite.Opacity = value;

            }
        }

        public Card()
        {
            random = new Random(); 
        }


        public void SwitchToFrontTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(FrontTexture, FrontTextureRect);
        }

        public void SwitchToBackTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(BackTexture, BackTextureRect);
        }




        public void FlipForever()
        {
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);
            CCCallFunc switchToBack = new CCCallFunc(this.SwitchToBackTexture);

            SwitchToBackTexture();
            var time = 0.5f;
            CCDelayTime pauseTime = new CCDelayTime(1f);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(time, 1, 0, 0, 90, 0, 0), switchToFront,
                    new CCOrbitCamera(time, 1, 0, -90, 90, 0, 0), pauseTime, new CCOrbitCamera(time, 1, 0, 0, 90, 0, 0), switchToBack,
                     new CCOrbitCamera(time, 1, 0, -90, 90, 0, 0), pauseTime

                    );

            CCRepeatForever flippingForever = new CCRepeatForever(flipSequence);
            RenderTexture.Sprite.RunAction(flippingForever);
            //RenderTexture.Sprite.RunAction(new CCOrbitCamera(10, 1, 0, 0, 180, 0, 0));


        }

        public async Task MoveBy(float duration, CCPoint targetPosition)
        {
            CCMoveBy moveToAction = new CCMoveBy(duration, targetPosition);
            var easeMoveTo = new CCEaseOut(moveToAction, 2f);
            await this.RunActionAsync(easeMoveTo);
        }
        public async Task MoveTo(float duration, CCPoint targetPosition)
        {
            CCMoveTo moveToAction = new CCMoveTo(duration, targetPosition);
            var easeMoveTo = new CCEaseOut(moveToAction, 2f);
            await this.RunActionAsync(easeMoveTo);
        }

        public async Task RotateTo(float duration, float deltaAngle)
        {
            CCRotateTo rotateToAction = new CCRotateTo(duration, deltaAngle);
            var easeMoveTo = new CCEaseOut(rotateToAction, 2f);
            await this.RunActionAsync(easeMoveTo);
        }
        public async Task RotateBy(float duration, float deltaAngle)
        {
            CCRotateBy rotateToAction = new CCRotateBy(duration, deltaAngle);
            var easeMoveTo = new CCEaseOut(rotateToAction, 2f);
            await this.RunActionAsync(easeMoveTo);
        }

        public async Task FlipToFront_RightToLeft(float duration)
        {
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, 0, 0, 90, 0, 0), switchToFront,
                    new CCOrbitCamera(duration, 1, 0, -90, 90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }
        public async Task FlipToFront(float duration)
        {
            
            float radius = 180;
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);
            CCSpawn spawn1 = new CCSpawn(new CCOrbitCamera(duration, 1, 0, 0, -90, 0, 0), new CCScaleBy(duration,0.85f));
            CCSpawn spawn2 = new CCSpawn(new CCOrbitCamera(duration, 1, 0, 90, -90, 0, 0), new CCScaleTo(duration,1f));

            CCSequence flipSequence = new CCSequence(spawn1, switchToFront, spawn2);
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }
        public async Task FlipToBack(float duration)
        {
            CCCallFunc switchToBack = new CCCallFunc(this.SwitchToBackTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, 0, 0, 90, 0, 0), switchToBack,
                    new CCOrbitCamera(duration, 1, 0, -90, 90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }

        public async Task RandomFlipOutOfScreen(float duration)
        {
            // bezier curve with endpoint = outofscreen
          


            CCPoint outOfScreenPoint = new CCPoint();
            if (random.Next(0, 2)==0)
            {
                outOfScreenPoint.X = random.Next(0, (int) GameConfig.ScreenSize.Width);
                outOfScreenPoint.Y = random.Next(0, 2) == 0 ? GameConfig.ScreenSize.Height + 80 : -80;
            }
            else
            {
                outOfScreenPoint.Y = random.Next(0, (int)GameConfig.ScreenSize.Height);
                outOfScreenPoint.X = random.Next(0, 2) == 0 ? GameConfig.ScreenSize.Width + 80 : -80;
            }
            
            CCOrbitCamera flip = new CCOrbitCamera(duration, 1, 0, 0, 300, 0, 200);
            RenderTexture.Sprite.RunActionAsync(flip);
            //this.MoveTo(duration, outOfScreenPoint);
            this.RunActionAsync(new CCEaseOut(new CCMoveTo(duration, outOfScreenPoint), 3f));
            this.RunActionAsync(new CCEaseOut(new CCScaleTo(duration, 0.6f), 2f));
            
            await this.FlipToBack(duration / 4f);
            await this.FlipToFront(duration / 4f);
            await this.FlipToBack(duration / 4f);
            await this.FlipToFront(duration / 4f);

        }


        public async Task Pause(float duration)
        {
            CCDelayTime delay = new CCDelayTime(duration);
            await RenderTexture.Sprite.RunActionAsync(delay);
        }

    }
}
