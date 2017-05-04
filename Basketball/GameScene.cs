using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

namespace Basketball
{
	public class Shelf : SKSpriteNode
	{
		bool isLeft;

		public Shelf(bool left, CGPoint point)
		{
			isLeft = left;
			this.Position = point;
			this.Texture = FromImageNamed("shelf").Texture;
		}
	}
	public class GameScene : SKScene
	{
		const int row = 6;
		const int col = 4;

		protected GameScene(IntPtr handle) : base(handle){}

		public override void DidMoveToView(SKView view)
		{
			Random rand = new Random();
			Shelf[][] matrix = new Shelf[row][];
			for (int i = 0; i < row; ++i) 
			{
				matrix[i] = new Shelf[col];
				for (int j = 0; j < col; ++j)
				{
					int temp = rand.Next(2);
					bool isLeft = false;
					if (temp == 0)
					{
						isLeft = true;
					}
				//	matrix[i][j] = new Shelf(isLeft, );
				}
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			foreach (var touch in touches)
			{
				var location = ((UITouch)touch).LocationInNode(this);
			}
		}

		public override void Update(double currentTime)
		{
			
		}
	}
}
