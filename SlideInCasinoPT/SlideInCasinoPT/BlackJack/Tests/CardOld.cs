using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.ViewModel
{
    public class CardOld : CCNode
    {
        // new for factory
        private CCTexture2D frontTexture, backTexture;
        private CCRect textureRect;




        /// <summary>
        /// 
        /// </summary>
        List<CCNode> visualComponents = new List<CCNode>();

        CCSprite background;
        CCSprite colorIconBig;
        CCSprite colorIconSmall;

        CCLabel valueDisplay;

        private CCSprite pokerBackSprite;
        private CCTexture2D cardFront;
        private CCRect cardFrontRect;

        CCRenderTexture RenderTexture;

        private string imageFile;


        //todo: check for null
        public CCPoint CardSize => new CCPoint(background.ContentSize.Width, background.ContentSize.Height);



        public override byte Opacity
        {
            get { return base.Opacity; }

            set
            {
                base.Opacity = value;
                this.RenderTexture.Sprite.Opacity = value;

            }
        }

        public CardOld(string value, string imageFile)
        {
            this.imageFile = imageFile;
            pokerBackSprite = new CCSprite("poker_back.png");




            CreateBackground();

            CreateColorIconBig();
            CreateColorIconSmall();
            CreateValueDisplay(value);

            AddComponentsToList();
            CreateRenderTexture();

            //SwitchToBackTexture();
        }

        private void CreateBackground()
        {
            background = new CCSprite("poker_bg.png");
            background.IsAntialiased = false;
            // The background serves as the largest sprite so it essentially defines the
            // card size and anchor point. Which is bottom left.
            background.AnchorPoint = CCPoint.Zero;
        }

        private void CreateColorIconBig()
        {
            colorIconBig = new CCSprite(imageFile);
            colorIconBig.PositionX = CardSize.X / 2;
            colorIconBig.PositionY = CardSize.Y / 2;
            colorIconBig.IsAntialiased = false;
        }

        protected void CreateColorIconSmall()
        {
            colorIconSmall = new CCSprite(imageFile);
            colorIconSmall.AnchorPoint = CCPoint.AnchorUpperLeft;
            colorIconSmall.Scale = 0.3f;
            //colorIconSmall.PositionX = colorIconSmall.ContentSize.Width/2f ;
            colorIconSmall.PositionX = 5;
            colorIconSmall.PositionY = CardSize.Y - 30;
            colorIconSmall.IsAntialiased = false;
        }


        private void CreateValueDisplay(string value)
        {
            valueDisplay = CreateLabel(value);
            valueDisplay.Color = CCColor3B.Red;
            valueDisplay.AnchorPoint = new CCPoint(0, .5f);
            valueDisplay.HorizontalAlignment = CCTextAlignment.Center;
            valueDisplay.PositionY = CardSize.Y - 15;
            valueDisplay.PositionX = 5;
        }


        private void AddComponentsToList()
        {
            visualComponents.Add(background);
            visualComponents.Add(colorIconBig);
            visualComponents.Add(colorIconSmall);
            visualComponents.Add(valueDisplay);
        }


        private void SwitchToFrontTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(cardFront, cardFrontRect);
        }

        private void SwitchToBackTexture()
        {
            RenderTexture.Sprite.ReplaceTexture(pokerBackSprite.Texture, pokerBackSprite.TextureRectInPixels);
        }



        private void CreateRenderTexture()
        {
            // The card needs to be moved to the origin (0,0) so it's rendered on the render target. 
            // After it's rendered to the CCRenderTexture, it will be moved back to its old position
            var oldPosition = this.Position;


            // Temporarily add them so we can render the object:
            foreach (var component in visualComponents)
            {
                this.AddChild(component);
            }


            // Create the render texture if it hasn't yet been made:
            if (RenderTexture == null)
            {
                // Even though the game is zoomed in to create a pixellated look, we are using
                // high-resolution textures. Therefore, we want to have our canvas be 2x as big as 
                // the background so fonts don't appear pixellated
                var unitResolution = background.ContentSize;
                var pixelResolution = background.ContentSize;
                RenderTexture = new CCRenderTexture(unitResolution, pixelResolution);
            }

            // We don't want the render target to be a child of the card 
            // when we call Visit
            if (this.Children != null && this.Children.Contains(RenderTexture.Sprite))
            {
                this.RemoveChild(RenderTexture.Sprite);
            }

            // Move this instance back to the origin so it is rendered inside the render target:
            this.Position = CCPoint.Zero;

            // Clears the CCRenderTexture
            RenderTexture.BeginWithClear(CCColor4B.Transparent);

            // Visit renders this object and all of its children
            this.Visit();

            // Ends the rendering, which means the CCRenderTexture's Sprite can be used
            RenderTexture.End();

            // We no longer want the individual components to be drawn, so remove them:
            foreach (var component in visualComponents)
            {
                this.RemoveChild(component);
            }

            // add the render target sprite to this:
            this.AddChild(RenderTexture.Sprite);

            RenderTexture.Sprite.AnchorPoint = CCPoint.AnchorMiddle;



            cardFront = RenderTexture.Texture;
            cardFrontRect = RenderTexture.Sprite.TextureRectInPixels;



        }

        private CCLabel CreateLabel(string text, int fontSize = 26)
        {
            var toReturn = new CCLabel(text, "Arial", fontSize, CCLabelFormat.SystemFont);
            toReturn.Scale = 1f;

            return toReturn;
        }

        public void FlipForever()
        {
            CCCallFunc switchToFront = new CCCallFunc(this.SwitchToFrontTexture);

            // CCCallFunc switchToFront2 = new CCCallFunc(() => RenderTexture.Sprite.ReplaceTexture(cardFront, cardFrontRect));

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
