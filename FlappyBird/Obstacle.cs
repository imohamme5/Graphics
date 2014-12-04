using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Obstacle
	{
		const float kGap = 300.0f;
		
		//Private variables.
		private SpriteUV[] 	sprites;
		private TextureInfo	textureInfoTop;
		private TextureInfo	textureInfoBottom;
		private float		width;
		private float		height;
		private bool 		hasPassed;
		
		//Accessors.
		//public SpriteUV SpriteTop 	 { get{return sprites[0];} }
		//public SpriteUV SpriteBottom { get{return sprites[1];} }
		
		//Public functions.
		public Obstacle (float startX, Scene scene)
		{
			
			textureInfoTop     = new TextureInfo("/Application/textures/toppipe.png");
			textureInfoBottom  = new TextureInfo("/Application/textures/bottompipe.png");
			
			sprites	= new SpriteUV[2];
			
			//Top
			sprites[0]			= new SpriteUV(textureInfoTop);	
			sprites[0].Quad.S 	= textureInfoTop.TextureSizef;
			//Add to the current scene.
			scene.AddChild(sprites[0]);
			
			//Bottom
			sprites[1]			= new SpriteUV(textureInfoBottom);	
			sprites[1].Quad.S 	= textureInfoBottom.TextureSizef;		
			//Add to the current scene.
			scene.AddChild(sprites[1]);
			
			//Get sprite bounds.
			Bounds2 b = sprites[0].Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			//Position pipes.
			sprites[0].Position = new Vector2(startX,
			                              Director.Instance.GL.Context.GetViewport().Height*RandomPosition());
			
			sprites[1].Position = new Vector2(startX, sprites[0].Position.Y-height-kGap);
		}
		
		public void Dispose()
		{
			textureInfoTop.Dispose();
			textureInfoBottom.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			sprites[0].Position = new Vector2(sprites[0].Position.X - 3, sprites[0].Position.Y);
			sprites[1].Position = new Vector2(sprites[1].Position.X - 3, sprites[1].Position.Y);
			
			//If off the left of the viewport, loop them around.
			if(sprites[0].Position.X < -width)
			{
				hasPassed = false;
				sprites[0].Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              Director.Instance.GL.Context.GetViewport().Height*RandomPosition());
			
				sprites[1].Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              sprites[0].Position.Y-height-kGap);
			}		
		}
		
		private float RandomPosition()
		{
			Random rand = new Random();
			float randomPosition = (float)rand.NextDouble();
			randomPosition += 0.45f;
			
			if(randomPosition > 1.0f)
				randomPosition = 0.9f;
		
			return randomPosition;
		}
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
		
			
			//top part
			if((sprite.Position.X + sprite.CalcSizeInPixels().X > sprites[0].Position.X && // check to see if birds right side is greater than pipes left side
			    sprites[0].Position.X + sprites[0].CalcSizeInPixels().X > sprite.Position.X && // check to see if pipes right side is greater than birds left side
				sprite.Position.Y + sprite.CalcSizeInPixels().Y > sprites[0].Position.Y))// check too see if birds top is greater than pipes bottom
			   return true;
			
			else if((sprite.Position.X + sprite.CalcSizeInPixels().X > sprites[1].Position.X &&
			         sprites[1].Position.X + sprites[0].CalcSizeInPixels().X > sprite.Position.X &&
				sprite.Position.Y < sprites[1].Position.Y + sprites[1].CalcSizeInPixels().Y))
				{
				return true;
				}
			
			//Alternative collison
			/*else if((sprite.Position.X + sprite.CalcSizeInPixels().X > sprites[1].Position.X && 
				sprite.Position.Y  < sprites[0].Position.Y-kGap))
				{
				return true;
				}*/
			
			else
			{
				return false;
			}
		}
		
//			public string score2(SpriteUV sprite)
//			{
//				if(sprite.Position.X  > sprites[0].Position.X)
//				{
//					score++;
//				}
//				
//				//return "score: " + score/17();
//			}
		
		public bool passed(SpriteUV sprite)
		{
			if(sprite.Position.X  > sprites[0].Position.X && !hasPassed)
			{
				hasPassed = true;
				return true;
			}
			else
				return false;
		}
			
	}
}

