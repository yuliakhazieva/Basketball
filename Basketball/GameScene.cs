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
			this.Texture = 
		}
	}
	//
	public class GameScene : SKScene
	{
		protected GameScene(IntPtr handle) : base(handle){}

		public override void DidMoveToView(SKView view)
		{
			
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
