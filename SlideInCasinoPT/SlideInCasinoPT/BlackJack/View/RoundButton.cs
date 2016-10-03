using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.View
{

    public delegate Task AsyncEventHandler();
    public class RoundButton : CCNode
    {


        private CCEventListenerTouchAllAtOnce touchListener;
        public event AsyncEventHandler Click;


        private CCDrawNode buttonBackground;
        private CCLabel textLabel;
        private CCRect ClickArea;
        private CCSprite heartSprite;
        public RoundButton(string text)
        {
            buttonBackground = new CCDrawNode();
            buttonBackground.DrawSolidCircle(
                pos: new CCPoint(0,0),
                radius: 80,
                color: new CCColor4B(255,255,255,50)
                );
            
            textLabel = CreateLabel(text);
            textLabel.Color = CCColor3B.White;

            heartSprite = new CCSprite("hearts_big.png");
            heartSprite.Scale = 3;
            heartSprite.Color = CCColor3B.Black;
            heartSprite.Opacity = 100;
            this.AddChild(heartSprite);
            // this.AddChild(buttonBackground);
            this.AddChild(textLabel);
            buttonBackground.AnchorPoint = CCPoint.AnchorMiddle;
            
            //this.AddChild(new CCSprite("hearts_big.png"));

            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            
            //ClickArea = buttonBackground.
            AddEventListener(touchListener, this);

            
        }

        private async void OnTouchesBegan(List<CCTouch> ccTouches, CCEvent ccEvent)
        {
            if (ccTouches.Count > 0)
            {
               
                CCTouch firstTouch = ccTouches[0];
      

               if (!alreadyTouched && heartSprite.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))              
                {
                    OnClick();
                }

            }
        }


        private CCLabel CreateLabel(string text, int fontSize = 24)
        {
            var toReturn = new CCLabel(text, "Arial", fontSize, CCLabelFormat.SystemFont);
            toReturn.Scale = 1f;

            return toReturn;
        }

        private bool alreadyTouched = false;
        protected virtual async Task OnClick()
        {
            if (this.Click != null)
            {
                foreach (var result in this.Click.GetInvocationList().Cast<AsyncEventHandler>())
                {
                    alreadyTouched = true;
                    await result();
                    alreadyTouched = false;
                }  
            }
            //Click?.Invoke(this, EventArgs.Empty);

        }
    }
}
