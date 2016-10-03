using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using CocosSharp;
using Microsoft.Xna.Framework.Graphics;
using SlideInCasinoPT.BlackJack.Model;
using SlideInCasinoPT.BlackJack.ViewModel;

namespace SlideInCasinoPT.BlackJack.View.Scenes.Layer
{

 
    public class SampleLayer : CCLayer
    {
        private CCSize screenSize;
        public SampleLayer(int width, int height)
        {
            screenSize = new CCSize(width, height);
            //InitMainGameObject();
            //DrawSprite();
 
        }






    



        protected  override async void AddedToScene()
        {
            base.AddedToScene();

            GameView.Stats.Enabled = true;

            var deckPos1 =  new CCPoint(screenSize.Width / 2, screenSize.Height * (1f / 3f));
            var deckPos2 =  new CCPoint(screenSize.Width / 2, screenSize.Height - screenSize.Height*(1f / 6f));

            TestPlayerDeck(deckPos1, 20);
            TestPlayerDeck(deckPos2, 10);
            //await TestDeckDraw();


        }

        private async Task TestPlayerDeck(CCPoint deckPosition, int cardCount)
        {
            PlayerDeck playerDeck = new PlayerDeck(false);
            playerDeck.DeckPosition = deckPosition;
            this.AddChild(playerDeck);

            for (int i = 0; i < cardCount; i++)
            {
                await playerDeck.DrawRandomCard();
                await this.RunActionAsync(new CCDelayTime(0.1f));
            }            
            


        }

        private async Task TestDeckDraw()
        {
            CCPoint DealerDeckPosition = new CCPoint(screenSize.Width/2f, screenSize.Height - (screenSize.Height*(1f/5f)));
            CCPoint PlayerDeckPosition = new CCPoint(screenSize.Width/2f, screenSize.Height*(0.8f/3f));
            await DrawDeck(7, DealerDeckPosition.X, DealerDeckPosition.Y, false);
            DrawDeck(10, PlayerDeckPosition.X, PlayerDeckPosition.Y, true);
        }

        private async Task DrawDeck(int cardCount, float x, float y, bool bowUp)
        {
        
            float bowValue = bowUp ? 1 : -1;

            float amplitude = 1f;

            var suits = Enum.GetValues(typeof(Suit));
            var cardValues = Enum.GetValues(typeof(CardValue));
            
            Random random = new Random();

            CCPoint addPos = new CCPoint(30, amplitude * bowValue);
            float addDegree = 5f * bowValue;

            CCPoint startPos = new CCPoint(x,y);
            float startDegree = 0;

            var end = (int) Math.Ceiling((cardCount/2f));
            var start = (int) Math.Floor(cardCount/2f);


            var deckPosition = new CCPoint(screenSize.Width, screenSize.Height);
            for (int i = -start+1; i < end+1; i++)
            {
                Suit randSuit = (Suit) suits.GetValue(random.Next(suits.Length));
                CardValue randCardValue = (CardValue) cardValues.GetValue(random.Next(cardValues.Length));

                Card card = CardFactory.Self.CreateNew(randSuit, randCardValue);
                
                card.AnchorPoint = CCPoint.AnchorMiddle;
                this.AddChild(card);


                //card.AnchorPoint = CCPoint.AnchorLowerLeft;
                var addingPos = new CCPoint(addPos.X * i, -(addPos.Y * i * i));

                var targetPosition = startPos + addingPos;
                card.Position = deckPosition;
                card.SwitchToBackTexture();

                card.Rotation = -90f;

                // Movement
                //card.Rotation = startDegree + (addDegree * i);
                var deltaDegree = startDegree + (addDegree * i);
                //card.Position = targetPosition;
                card.RotateTo(0.3f, deltaDegree);
                await card.MoveTo(0.3f, targetPosition);


                await card.Pause(0.1f);
                await card.FlipToFront(0.15f);
                await card.Pause(0.3f);


            }

        }

       
    }
}
