using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using SlideInCasinoPT.BlackJack.ViewModel;

namespace SlideInCasinoPT.BlackJack.Model
{
    public class CardFactory
    {
        private static Lazy<CardFactory> self = new Lazy<CardFactory>(() => new CardFactory());
        public static CardFactory Self => self.Value;


        private Dictionary<Suit, string> imageFileMap;
        private Dictionary<CardValue, string> cardTextMap;

        private string bgImageFile = "card_empty.png";
        private string cardBackImageFile = "card_back.png";


        private CardFactory()
        {
            CreateImageFileMap();
            CreateCardTextMap();
        }

        private void CreateCardTextMap()
        {
            cardTextMap = new Dictionary<CardValue, string>
            {
                [CardValue.Two] = "2",
                [CardValue.Three] = "3",
                [CardValue.Four] = "4",
                [CardValue.Five] = "5",
                [CardValue.Six] = "6",
                [CardValue.Seven] = "7",
                [CardValue.Eight] = "8",
                [CardValue.Nine] = "9",
                [CardValue.Ten] = "10",
                [CardValue.Jack] = "J",
                [CardValue.Queen] = "Q",
                [CardValue.King] = "K",
                [CardValue.As] = "A"

            };
        }

        private void CreateImageFileMap()
        {
            imageFileMap = new Dictionary<Suit, string>
            {
                [Suit.Clubs] = "clubs_big.png",
                [Suit.Diamonds] = "diamonds_big.png",
                [Suit.Hearts] = "hearts_big.png",
                [Suit.Spades] = "spades_big.png"

            };
        }


        public Card CreateNew(Suit suit, CardValue value)
        {
            var imageFile = imageFileMap[suit];
            var cardText = cardTextMap[value];

            Card card = new Card();
            CreateVisualComponents(card, cardText,suit, imageFile);
            card.BackTexture = CCTextureCache.SharedTextureCache.AddImage(cardBackImageFile);
            return card;
        }

        private void CreateVisualComponents(Card card, string cardText, Suit suit, string imageFile)
        {
            List<CCNode> visualComponents = new List<CCNode>();
            //Create Background Sprite
            CCSprite backgroundSprite = CreateBackground();
            card.BackTextureRect = backgroundSprite.TextureRectInPixels;
            CCSize cardSize = backgroundSprite.ContentSize;

            CCSprite colorIconBig = CreateColorIconBig(imageFile, cardSize);
            CCSprite colorIconSmall = CreateColorIconSmall(imageFile, cardSize);
            CCLabel cardTextDisplay = CreatCardTextDisplay(cardText, cardSize, suit);

            visualComponents.Add(backgroundSprite);
            visualComponents.Add(colorIconBig);
            visualComponents.Add(colorIconSmall);
            visualComponents.Add(cardTextDisplay);


            CreateRenderTexture(card, visualComponents, cardSize);



        }

        private CCSprite CreateBackground()
        {
            
            CCSprite background = new CCSprite(bgImageFile);
            
            background.IsAntialiased = false;
            // The background serves as the largest sprite so it essentially defines the
            // card size and anchor point. Which is bottom left.
            background.AnchorPoint = CCPoint.Zero;
            
            //background.TextureRectInPixels = new CCRect(background.TextureRectInPixels.MinX, background.TextureRectInPixels.MinY,
            //    background.TextureRectInPixels.MinX + (2 * background.TextureRectInPixels.Size.Width),
            //    background.TextureRectInPixels.MinY + (2 * background.TextureRectInPixels.Size.Height));
            return background;
        }

        private CCSprite CreateColorIconBig(string imageFile, CCSize cardSize)
        {
            CCSprite colorIconBig = new CCSprite(imageFile);
            colorIconBig.PositionX = cardSize.Width/2;
            colorIconBig.PositionY = cardSize.Height/2;
            colorIconBig.IsAntialiased = false;
            return colorIconBig;
        }

        private CCSprite CreateColorIconSmall(string imageFile, CCSize cardSize)
        {
            CCSprite colorIconSmall = new CCSprite(imageFile);
            colorIconSmall.AnchorPoint = CCPoint.AnchorUpperLeft;
            colorIconSmall.Scale = 0.3f;
            colorIconSmall.PositionX = 25;
            colorIconSmall.PositionY = cardSize.Height - 65;
            colorIconSmall.IsAntialiased = false;
            return colorIconSmall;
        }

        private CCLabel CreatCardTextDisplay(string cardText, CCSize cardSize, Suit suit)
        {
            CCLabel cardTextDisplay = CreateLabel(cardText);
            if(suit == Suit.Diamonds || suit == Suit.Hearts)
                cardTextDisplay.Color = CCColor3B.Red;
            else
            {
                cardTextDisplay.Color = CCColor3B.Black;
            }
            cardTextDisplay.AnchorPoint = new CCPoint(0, .5f);
            cardTextDisplay.HorizontalAlignment = CCTextAlignment.Center;
            cardTextDisplay.PositionY = cardSize.Height - 50;
            cardTextDisplay.PositionX = 25;

            return cardTextDisplay;
        }


        private CCLabel CreateLabel(string text, int fontSize = 24)
        {
            var toReturn = new CCLabel(text, "Arial", fontSize, CCLabelFormat.SystemFont);
            toReturn.Scale = 1f;

            return toReturn;
        }

        private void CreateRenderTexture(Card card, List<CCNode> visualComponents, CCSize cardSize)
        {
            // Temporarily add them so we can render the object:
            foreach (var component in visualComponents)
            {
                card.AddChild(component);
            }


            // Create the render texture if it hasn't yet been made:
            if (card.RenderTexture == null)
            {
                var unitResolution = cardSize ;
                var pixelResolution = cardSize * 2;
                card.RenderTexture = new CCRenderTexture(unitResolution, pixelResolution);
            }

            // We don't want the render target to be a child of the card 
            // when we call Visit
            if (card.Children != null && card.Children.Contains(card.RenderTexture.Sprite))
            {
                card.RemoveChild(card.RenderTexture.Sprite);
            }

            // Move this instance back to the origin so it is rendered inside the render target:
            card.Position = CCPoint.Zero;

            // Clears the CCRenderTexture
            card.RenderTexture.BeginWithClear(CCColor4B.Transparent);

            // Visit renders this object and all of its children
            card.Visit();

            // Ends the rendering, which means the CCRenderTexture's Sprite can be used
            card.RenderTexture.End();

            // We no longer want the individual components to be drawn, so remove them:
            foreach (var component in visualComponents)
            {
                card.RemoveChild(component);
            }

            // add the render target sprite to this:
            card.AddChild(card.RenderTexture.Sprite);

            card.RenderTexture.Sprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            card.RenderTexture.Sprite.Position = CCPoint.Zero;

            card.RenderTexture.Sprite.AnchorPoint = CCPoint.AnchorMiddle;
            card.FrontTexture = card.RenderTexture.Texture;
            card.FrontTextureRect = card.RenderTexture.Sprite.TextureRectInPixels;



        }

    }
}
