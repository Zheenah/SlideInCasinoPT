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
        public CCRect TextureRect;


        private string imageFile;


        public CCRenderTexture RenderTexture;

        
       

       //todo: check for null
        //public CCPoint CardSize => new CCPoint(background.ContentSize.Width, background.ContentSize.Height);



        public override byte Opacity
        {
            get { return base.Opacity; }

            set
            {
                base.Opacity = value;
                this.RenderTexture.Sprite.Opacity = value;

            }
        }

        public Card(string value, string imageFile)
        {
            this.imageFile = imageFile;

            



            //SwitchToBackTexture();
        }


        private void SwitchToFrontTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(FrontTexture, TextureRect);
        }

        private void SwitchToBackTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(BackTexture, TextureRect);
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

        public async Task FlipToFront(float duration)
        {
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, 0, 0, 90, 0, 0), switchToFront,
                    new CCOrbitCamera(duration, 1, 0, -90, 90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }
        public async Task FlipToBack(float duration)
        {
            CCCallFunc switchToBack = new CCCallFunc(this.SwitchToBackTexture);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(duration, 1, 0, 0, 90, 0, 0), switchToBack,
                    new CCOrbitCamera(duration, 1, 0, -90, 90, 0, 0));
            await RenderTexture.Sprite.RunActionAsync(flipSequence);

        }

    }
}
