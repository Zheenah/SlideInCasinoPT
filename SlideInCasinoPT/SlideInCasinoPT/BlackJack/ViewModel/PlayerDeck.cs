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
        private CCPoint DeckPosition;
        private CCPoint PositionSplit1, PositionSplit2;

        private List<Card> deck1;

        float amplitude = 1f;
  
        private bool bowUp = true;

        private CCPoint distBetweenCards;
        float angleBetweenCards;


        private bool split = false;

        private CCSize screenSize;

        public PlayerDeck()
        {
           
            this.screenSize = GameConfig.ScreenSize;


            DeckPosition = new CCPoint(screenSize.Width/2, screenSize.Height * (1f/3f));

            int bowValue = bowUp ? 1 : -1;

            angleBetweenCards = 5f * bowValue;
            distBetweenCards = new CCPoint(30, amplitude * bowValue);

            deck1 = new List<Card>();

        }

        public async Task DrawCard(Card card)
        {
            // Move all cards one position to left
            if (deck1.Count%2 == 1)
            {
               await MoveCardsToLeft(0.1f);

            }

            // Start Position, Deck DeckPosition and Rotation
            card.PositionX = screenSize.Width ;
            card.PositionY = screenSize.Height;
            card.Rotation = -90;
            card.SwitchToBackTexture();
            this.AddChild(card);
            // Move to right Position
            int deckPos = (deck1.Count / 2) ;


           var addingPos = new CCPoint(distBetweenCards.X * deckPos, -(distBetweenCards.Y * deckPos * deckPos));
           
            //var addingPos = new CCPoint(distBetweenCards.X * deckPos, 0);
            var targetPosition = DeckPosition + addingPos;
            var targetAngle =  (angleBetweenCards * deckPos);

            card.RotateTo(1f, targetAngle);
            await card.MoveTo(1f, targetPosition);


            await card.Pause(0.1f);
            await card.FlipToFront(0.15f);
            await card.Pause(0.3f);


            deck1.Add(card);
            // Add new card

        }

        private async Task MoveCardsToLeft(float duration)
        {


            for (int i = 0; i < deck1.Count; i++)
            {
                Card card = deck1[i];
                //todo: calc i
                var x = i - (deck1.Count/2f) - 1;
                var newPosX = card.PositionX - distBetweenCards.X;
                //card.MoveBy(0.1f, new CCPoint(-distBetweenCards.X, 0));

                //var newPosY = card.PositionY - distBetweenCards.Y*x*x;
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
