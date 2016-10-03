using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using SlideInCasinoPT.BlackJack.ViewModel;

namespace SlideInCasinoPT.BlackJack.View.Scenes.Layer
{
    public class GameLayer : CCLayer
    {
        private CCSize screenSize;
        private PlayerDeck playerDeck, dealerDeck;
        public GameLayer()
        {
            screenSize = GameConfig.ScreenSize;
        }
        protected override async void AddedToScene()
        {
            base.AddedToScene();
            CCLog.Log("Game Layer added to Scene");
            GameView.Stats.Enabled = true;

            var playerDeckPos = new CCPoint(screenSize.Width / 2, screenSize.Height * (1f / 3f));
            var dealerDeckPos = new CCPoint(screenSize.Width / 2, screenSize.Height - screenSize.Height * (1f / 6f));

            //TestPlayerDeck(playerDeckPos, 20);
            //TestPlayerDeck(dealerDeckPos, 10);

            playerDeck = new PlayerDeck(true);
            playerDeck.DeckPosition = playerDeckPos;
            this.AddChild(playerDeck);

            dealerDeck = new PlayerDeck(false);
            dealerDeck.DeckPosition = dealerDeckPos;
            this.AddChild(dealerDeck);


        }

        public async Task RemoveCards()
        {
             await RemoveDealerCards();
             await  RemovePlayerCards();
        }

        private async Task RemovePlayerCards()
        {
            var mid = screenSize.Width / 2f;
            foreach (var card in playerDeck.Deck1)
            {

                var targetPosX = card.PositionX - mid;
                var targetDir = Math.Sign(targetPosX);
                CCPoint targetPos = new CCPoint(card.PositionX,-80);
                //card.MoveTo(0.3f, targetPos);
                card.RandomFlipOutOfScreen(1f);


            }
            await this.RunActionAsync(new CCDelayTime(1f));
            foreach (var card in playerDeck.Deck1)
            {
                playerDeck.RemoveChild(card);

            }
            playerDeck.Deck1 = new List<Card>();
        }

        private async Task RemoveDealerCards()
        {
            var mid = screenSize.Width / 2f;
            foreach (var card in dealerDeck.Deck1)
            {
                
                var targetPosX = card.PositionX - mid;
                var targetDir = Math.Sign(targetPosX);
                CCPoint targetPos = new CCPoint(card.PositionX , screenSize.Height + 80);
                card.MoveTo(0.3f, targetPos);               
                
            }
            await this.RunActionAsync(new CCDelayTime(0.3f));
            foreach (var card in dealerDeck.Deck1)
            {
                dealerDeck.RemoveChild(card);

            }

            dealerDeck.Deck1 = new List<Card>();

        }


        public async Task DealerDrawCard()
        {
            await dealerDeck.DrawRandomCard();
        }

        public async Task PlayerDrawCard()
        {
            await playerDeck.DrawRandomCard();
        }

        private async Task TestPlayerDeck(CCPoint deckPosition, int cardCount)
        {
            PlayerDeck playerDeck = new PlayerDeck(true);
            playerDeck.DeckPosition = deckPosition;
            this.AddChild(playerDeck);

            for (int i = 0; i < cardCount; i++)
            {
                await playerDeck.DrawRandomCard();
               // await this.RunActionAsync(new CCDelayTime(0.1f));
            }



        }
    }
}
