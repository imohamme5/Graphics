using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace FlappyBird
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		public static int ScreenWidth;
		public static int ScreenHeight;
		private static Obstacle[]	obstacles;
		private static Bird			bird;
		private static Background	background;
		private static Sun	sunny;
		
		private static int score = 0; // 
		
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			bird.Dispose();
			foreach(Obstacle obstacle in obstacles)
				obstacle.Dispose();
			background.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
	
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			ScreenWidth = Director.Instance.GL.Context.Screen.Width;
			ScreenHeight = Director.Instance.GL.Context.Screen.Height;
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.Text = "0";
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//create the sun
			sunny = new Sun(gameScene);
			
			//Create the flappy douche
			bird = new Bird(gameScene);
			
			//Create some obstacles.
			obstacles = new Obstacle[2];
			obstacles[0] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width*0.5f, gameScene);	
			obstacles[1] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width, gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			//If tapped, inform the bird.
			if(touches.Count > 0)
				bird.Tapped();
			
			//Update the bird.
			bird.Update(0.0f);
			
			if(bird.Alive)
			{
				//Move the background.
				background.Update(0.0f);
				
				sunny.Update(0.0f, bird.Sprite);
							
				//Update the obstacles.
				
				foreach(Obstacle obstacle in obstacles)
				{
				obstacle.Update(0.0f);
					if(obstacle.passed(bird.Sprite))
					{
						score += 1;
						scoreLabel.Text = score.ToString();
					}
					//score
					//scoreLabel.Text = obstacle.score2(bird.Sprite);
				
				//check if obstacle has collided
				if(obstacle.HasCollidedWith(bird.Sprite))
					{
						bird.Alive = false;
						Console.WriteLine("COLLIDED");
						
					}
					 
					
				 
				}
				
			}
		}
		
	}
}
