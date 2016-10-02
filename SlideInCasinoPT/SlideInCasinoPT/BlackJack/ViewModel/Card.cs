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
        // new for factory
        public CCTexture2D FrontTexture, BackTexture;
        public CCRect FrontTextureRect, BackTextureRect;


        public CCRenderTexture RenderTexture;

       

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
            float radius = 60;
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, radius, 0, -90, 0, 0), switchToFront,
                    new CCOrbitCamera(duration, radius, -(radius-1), 90, -90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }
        public async Task FlipToBack(float duration)
        {
            CCCallFunc switchToBack = new CCCallFunc(this.SwitchToBackTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, 0, 0, 90, 0, 0), switchToBack,
                    new CCOrbitCamera(duration, 1, 0, -90, 90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }

        public async Task Pause(float duration)
        {
            CCDelayTime delay = new CCDelayTime(duration);
            await RenderTexture.Sprite.RunActionAsync(delay);
        }

    }
}
