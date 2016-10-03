using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace SlideInCasinoPT.BlackJack.View.Scenes.Layer
{
    public class UiLayer : CCLayer
    {
        private GameLayer gameLayer;
        public UiLayer(GameLayer gameLayer)
        {
            this.gameLayer = gameLayer;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            CreateUi();
        }

        private void CreateUi()
        {
            RoundButton playerDrawCardButton = new RoundButton("Player:\nDraw Card");
            playerDrawCardButton.Position = new CCPoint(GameConfig.ScreenSize.Width / 5,200);
            playerDrawCardButton.Click += PlayerDrawCardButton_Click;
            this.AddChild(playerDrawCardButton);

            RoundButton dealerDrawCardButton = new RoundButton("Dealer:\nDraw Card");
            dealerDrawCardButton.Position = new CCPoint(4 * GameConfig.ScreenSize.Width / 5,200);
            dealerDrawCardButton.Click += DealerDrawCardButton_Click;
            this.AddChild(dealerDrawCardButton);

            RoundButton removeCardsButton = new RoundButton("Remove\nCards");
            removeCardsButton.Position = new CCPoint(GameConfig.ScreenSize.Width / 2, 600);
            removeCardsButton.Click += RemoveCardsButton_Click;
            this.AddChild(removeCardsButton);


        }

        private async Task RemoveCardsButton_Click()
        {
            await gameLayer.RemoveCards();
        }

        private async Task DealerDrawCardButton_Click()
        {
            await gameLayer.DealerDrawCard();
        }

        private async Task PlayerDrawCardButton_Click()
        {
           await  gameLayer.PlayerDrawCard();
        }
    }
}
