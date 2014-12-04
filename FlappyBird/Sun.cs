using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input; // for touch input

namespace FlappyBird
{
	public class Sun
	{
		//private variables
		private SpriteUV[] sun;
		private TextureInfo	text;
		private TextureInfo	text2;
		private float		width;
	 	private bool hasCollected  	= false;
		private int 		i;
		private Scene theScene;
		private bool isAlive = true;
		float screenX, screenY;
		public Sun (Scene scene)
		{
			this.theScene = scene;
			i = 1;			
			sun = new SpriteUV[2];
			
			text = new TextureInfo("/Application/textures/bird2.png");
			text2 = new TextureInfo("/Application/textures/bird2.png");
			
			//texture for sun
			sun[0] = new SpriteUV(text);
			sun[0].Quad.S = text.TextureSizef;
			
			//position sun
			sun[0].Position = new Vector2(200.0f, 150.0f);
			
			sun[1] = new SpriteUV(text2);
			sun[1].Quad.S = text2.TextureSizef;
			
			sun[1].Position = new Vector2(300.0f, 150.0f);
			
			//Add to the current scene.
			foreach(SpriteUV sprite in sun)
			scene.AddChild(sprite);
			//scene.AddChild(sun[0]);
			 
			
			 
		}
		
		public void Dispose()
		{
			text.Dispose();
		}
		
		public void Update(float deltaTime, SpriteUV sprite)
		{
			var touches = Touch.GetData(0);
			Rectangle touchRect = new Rectangle();
			//Console.WriteLine(sun[0].Position);
			for(int i = 0; i< touches.Count; i++)
			{
				screenX = (touches[i].X + 0.5f) * AppMain.ScreenWidth;
				screenY = (touches[i].X + 0.5f) * AppMain.ScreenHeight;
				touchRect.X = screenX;
				touchRect.Y = screenY;
				touchRect.Width = 1;
				touchRect.Height = 1;
				//Console.WriteLine(screenX + ":" +screenY);
				
			}
			
			
			Rectangle sunRect = new Rectangle();
			sunRect.X = sun[0].Position.X;
			sunRect.Y = sun[0].Position.Y;
			sunRect.Width = sun[0].CalcSizeInPixels().X;
			sunRect.Height = sun[0].CalcSizeInPixels().Y;		
			
			if((touchRect.X > sunRect.X) && (touchRect.X < sunRect.X + sunRect.Width) )
			{
				resetSun();
			}
			
			sun[0].Position = new Vector2(sun[0].Position.X - 2.0f, sun[0].Position.Y); //setting new position for sun
			
			if(sun[0].Position.X < -width) // if sun is less than width of screen call method
			{
			 resetSun();
			}
			else if(sprite.Position.X + sprite.CalcSizeInPixels().X > sun[0].Position.X
			        && sun[0].Position.X + sun[0].CalcSizeInPixels().X > sprite.Position.X //collision detection
			        && !hasCollected)	 // no way for it to be colelcted
			{
				hasCollected = true; // change when it has collected
				i++;
				Console.WriteLine(i);
				resetSun();
				 
			}
			if(!isAlive)
			{
				resetSun();
				
			}
						
		}
		
		public void resetSun()
		{
			sun[0].Position =  new Vector2(Director.Instance.GL.Context.GetViewport().Width + 20.0f,  250.0f); //set new position of sun in middle of screen
			hasCollected = false; // reset sun
		}
	}
}

