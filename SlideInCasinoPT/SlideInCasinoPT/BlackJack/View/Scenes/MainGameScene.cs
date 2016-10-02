using CocosSharp;
using SlideInCasinoPT.BlackJack.View.Scenes.Layer;

namespace SlideInCasinoPT.BlackJack.View.Scenes
{
    public class MainGameScene : CCScene
    {
       
        public MainGameScene(CCGameView gameView, int width, int height) : base(gameView)
        {
            GameConfig.ScreenSize = new CCSize(width, height);
            var backgroundLayer = new BackgroundLayer(width, height);
            this.AddLayer(backgroundLayer);
            var sampleLayer = new SampleLayer(width, height);
            this.AddLayer(sampleLayer);
            
        }
        
    }
}
