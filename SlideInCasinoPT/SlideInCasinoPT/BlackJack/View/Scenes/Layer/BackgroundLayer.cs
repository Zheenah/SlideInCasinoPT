using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.View.Scenes.Layer
{
    public class BackgroundLayer : CCLayer
    {
        private int width, height;
        public BackgroundLayer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            CCSprite background = new CCSprite("table_bg.png");
            background.IsAntialiased = true;
            background.AnchorPoint = CCPoint.AnchorLowerLeft;
            background.Position = CCPoint.Zero;

            background.ScaleX = width/background.ContentSize.Width;
            background.ScaleY = height/background.ContentSize.Height;

            //background.TextureRectInPixels = new CCRect(0,0,width, height);
            this.AddChild(background);
        }
    }
}
