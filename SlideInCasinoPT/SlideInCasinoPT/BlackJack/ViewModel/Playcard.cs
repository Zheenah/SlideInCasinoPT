using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.ViewModel
{
    public class Playcard : CCNode
    {
        private CCSprite sprite;
        private CCEventListenerTouchAllAtOnce touchListener;


        private bool useRenderTexture;
        List<CCNode> visualComponents = new List<CCNode>();
        private CCSprite background;
        private CCSprite icon;
        private CCLabel numberDisplay;

        public Playcard() : base()
        {
           

            sprite = new CCSprite("poker_bg.png");
            
            sprite.AnchorPoint = CCPoint.AnchorMiddle;
            this.AddChild(sprite);


            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
            
        }



        private void OnTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch firstTouch = touches[0];

                this.PositionX = firstTouch.Location.X;
                this.PositionY = firstTouch.Location.Y;
            }
        }
    }
}
