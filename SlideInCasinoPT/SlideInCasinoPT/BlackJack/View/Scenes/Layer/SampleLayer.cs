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
        public SampleLayer()
        {

            //InitMainGameObject();
            //DrawSprite();
 
        }

        public void InitMainGameObject()
        {
            Playcard card1 = new Playcard();
            this.AddChild(card1);
            card1.Position = new CCPoint(300,300);
        }

        public void DrawSprite()
        {
            CCSprite sprite = new CCSprite("poker_back.png");
             
            this.AddChild(sprite);
            sprite.Position = new CCPoint(200,200);
            //sprite.Scale = 3f;

            CCAction right = new CCMoveBy(5,new CCPoint(400,0));
            CCAction up = new CCMoveBy(3, new CCPoint(0,400));
            //sprite.RunActions(up, right);

            CCFlipY flipY = new CCFlipY(true);
           CCFlipX3D flipx3 = new CCFlipX3D(5, new CCGridSize(1,1));
            
            CCTexture2D pokerFrontText = new CCTexture2D();
            //sprite.ReplaceTexture(spriteFront.Texture,sprite.TextureRectInPixels);
            
            FlipCard(sprite);


        }


        public async void FlipCard(CCSprite card)
        {
            //CCOrbitCamera flipAction = new CCOrbitCamera(5.0f, 1, 0, 0, 90, 0, 0);
            //await card.RunActionAsync(new CCOrbitCamera(5.0f, 1, 0, 0, 90, 0, 0));
            CCSprite spriteFront = new CCSprite("poker_bg.png");
            CCTexture2D cardFrontTexture = spriteFront.Texture;
            CCTexture2D cardBackTexture = card.Texture;

            
            CCCallFunc switchToFront =  new CCCallFunc( () => card.ReplaceTexture(cardFrontTexture, card.TextureRectInPixels));
            CCCallFunc switchToBack = new CCCallFunc(() => card.ReplaceTexture(cardBackTexture, card.TextureRectInPixels));
            ////card.ReplaceTexture(spriteFront.Texture, card.TextureRectInPixels);
            //await card.RunActionAsync(new CCOrbitCamera(5.0f, 1, 0, 90, 90, 0, 0));
            var time = 0.5f;
            CCDelayTime pauseTime = new CCDelayTime(1f);
            CCSequence flipSequence = new CCSequence(new CCOrbitCamera(time, 1, 0, 0, 90, 0, 0), switchToFront,
                    new CCOrbitCamera(time, 1, 0, 90, 90, 0, 0), pauseTime,new CCOrbitCamera(time , 1, 0, 180, 90, 0, 0), switchToBack,
                     new CCOrbitCamera(time, 1, 0, 270, 90, 0, 0), pauseTime

                    );

            CCRepeatForever flippingForever = new CCRepeatForever(flipSequence);
            card.RunAction(flippingForever);
        }


    


        public void DrawCircle()
        {
            CCDrawNode circle = new CCDrawNode();

            this.AddChild(circle);
            circle.DrawCircle(
                // The center to use when drawing the circle,
                // relative to the CCDrawNode:
                new CCPoint(0, 0),
                radius: 15,
                color: CCColor4B.White);
            circle.PositionX = 20;
            circle.PositionY = 50;
            CCRect rectToDraw = new CCRect(10,10,50,50);
            
            circle.DrawRect(rectToDraw);
        }


        protected override void AddedToScene()
        {
            base.AddedToScene();

            GameView.Stats.Enabled = true;

            //await CardTestStuff();

            TestNewCardStuff();
        }

        private void TestNewCardStuff()
        {
            Card card = CardFactory.Self.CreateNew(Suit.Diamonds, CardValue.Jack);
            this.AddChild(card);
            card.Position = new CCPoint(200,200);
        }

        private async Task CardTestStuff()
        {
            const bool useRenderTextures = true;
            const byte opacity = 255;

            const float positionIncrement = 140;

            Card card = new Card("A", "karo_big.png");
            //card.ColorIconBigTexture = CCTextureCache.SharedTextureCache.AddImage("karo_big.png");
            card.PositionX = 200;
            card.PositionY = 400;

            card.Opacity = opacity;
            this.AddChild(card);


            Card card2 = CardFactory.Self.CreateNew(Suit.Diamonds, CardValue.Jack);
            this.AddChild(card2);

            card2.Position = new CCPoint(350, 350);

            await card.FlipToFront(4);
            await card.FlipToBack(3);
        }
    }
}
