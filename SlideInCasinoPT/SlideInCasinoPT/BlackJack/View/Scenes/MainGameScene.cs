using CocosSharp;
using SlideInCasinoPT.BlackJack.View.Scenes.Layer;

namespace SlideInCasinoPT.BlackJack.View.Scenes
{
    public class MainGameScene : CCScene
    {
       
        public MainGameScene(CCGameView gameView) : base(gameView)
        {
            var sampleLayer = new SampleLayer();
            this.AddLayer(sampleLayer);
            
        }
    }
}
