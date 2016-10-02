using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using CocosSharp;
using SlideInCasinoPT.BlackJack;
using SlideInCasinoPT.BlackJack.View.Scenes;

namespace SlideInCasinoPT.Droid
{
	[Activity (Label = "SlideInCasinoPT.Droid", MainLauncher = true,
        Theme = "@style/Theme.RedGrey", Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation,
         ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

//			SetContentView (Resource.Layout.Main);
            SetContentView (Resource.Layout.Overlay);

            //InitWebView();
            //InitOverlay();
		    InitGame();


		}

	    private void InitGame()
	    {
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);

            if (gameView != null)
                gameView.ViewCreated += LoadGame;
        }

	    private void LoadGame(object sender, EventArgs e)
	    {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null)
            {
              // Set world dimensions
                
                int width = Resources.DisplayMetrics.WidthPixels  / 2 ;
                int height = Resources.DisplayMetrics.HeightPixels / 2;

                var contentSearchPaths = new List<string>() { "Fonts", "Sounds", "Images" };


                gameView.DesignResolution = new CCSizeI(width, height);
                gameView.ContentManager.SearchPaths = contentSearchPaths;

                CCScene gameScene = new MainGameScene(gameView);
                gameView.RunWithScene(gameScene);
            }
        }


	    private void InitOverlay()
	    {
            var overlayLayout = FindViewById<LinearLayout>(Resource.Id.OverlayLayout);
            var metrics = Resources.DisplayMetrics;
            overlayLayout.LayoutParameters.Width = metrics.WidthPixels;

        }

	    private void InitWebView()
	    {
            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new WebViewClient());

            // For better performance
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
            webView.Settings.SetAppCacheEnabled(true);
            webView.Settings.DomStorageEnabled = true;
            webView.Settings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.NarrowColumns);
            webView.Settings.UseWideViewPort = true;
            webView.Settings.SavePassword = true;
            webView.Settings.SaveFormData = true;
            webView.Settings.EnableSmoothTransition();


            webView.LoadUrl("https://m.tipico.de/?demo=true&scroll=fixed&client=android");
            //webView.LoadUrl("javascript:document.write(navigator.userAgent)");
        }
	}
}


