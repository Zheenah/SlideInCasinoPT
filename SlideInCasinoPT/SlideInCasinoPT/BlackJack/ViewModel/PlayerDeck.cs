using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using SlideInCasinoPT.BlackJack.Model;

namespace SlideInCasinoPT.BlackJack.ViewModel
{
    public class PlayerDeck : CCNode
    {
        public CCPoint DeckPosition;
        private CCPoint PositionSplit1, PositionSplit2;

        public List<Card> Deck1;

        float amplitude = 1f;
  
        private bool bowUp = true;

        private CCPoint distBetweenCards;
        float angleBetweenCards;


        private bool split = false;

        private CCSize screenSize;

        public PlayerDeck(bool bowUp)
        {
            DeckPosition = new CCPoint(screenSize.Width/2, screenSize.Height * (1f/3f));

            this.screenSize = GameConfig.ScreenSize;
            

            int bowValue = bowUp ? 1 : -1;

            angleBetweenCards = 5f * bowValue;
            distBetweenCards = new CCPoint(30, amplitude * bowValue);

            Deck1 = new List<Card>();

        }



        public async Task DrawCard(Card card)
        {
            // Move all cards one position to left
            if (Deck1.Count%2 == 1)
            {
               await MoveCardsToLeft(0.2f);

            }

            // Start Position, Deck DeckPosition and Rotation
            card.PositionX = screenSize.Width ;
            card.PositionY = screenSize.Height;
            card.Rotation = -90;
            card.SwitchToBackTexture();
            this.AddChild(card);
            // Move to right Position
            int deckPos = (Deck1.Count / 2) ;


           var addingPos = new CCPoint(distBetweenCards.X * deckPos, -(distBetweenCards.Y * deckPos * deckPos));
           
            //var addingPos = new CCPoint(distBetweenCards.X * deckPos, 0);
            var targetPosition = DeckPosition + addingPos;
            var targetAngle =  (angleBetweenCards * deckPos);

            card.RotateTo(0.5f, targetAngle);
            await card.MoveTo(0.5f, targetPosition);


            await card.Pause(0.1f);
            await card.FlipToFront(0.15f);
            //await card.Pause(0.3f);


            Deck1.Add(card);
            // Add new card

        }

        private async Task MoveCardsToLeft(float duration)
        {


            for (int i = 0; i < Deck1.Count; i++)
            {
                Card card = Deck1[i];
                var x = i - (Deck1.Count/2f) - 1;
                var newPosX = card.PositionX - distBetweenCards.X;
                var newPosY = DeckPosition.Y - distBetweenCards.Y * x * x;
                card.MoveTo(duration, new CCPoint(newPosX, newPosY));
                card.RotateBy(duration, -angleBetweenCards);
                
            }
            await this.RunActionAsync(new CCDelayTime(duration));



        }

        public async Task DrawRandomCard()
        {
            // Create Random Card
            var suits = Enum.GetValues(typeof(Suit));
            var cardValues = Enum.GetValues(typeof(CardValue));
            Random random = new Random();

            Suit randSuit = (Suit)suits.GetValue(random.Next(suits.Length));
            CardValue randCardValue = (CardValue)cardValues.GetValue(random.Next(cardValues.Length));
            Card card = CardFactory.Self.CreateNew(randSuit, randCardValue);
            card.AnchorPoint = CCPoint.AnchorMiddle;
            

            await DrawCard(card);
            //this.AddChild(card);

        }
    }
}
