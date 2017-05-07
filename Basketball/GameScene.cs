using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

namespace Basketball
{
	enum gameState { welcome, play, pause, tryAgain}

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
		gameState thisGame;
		protected GameScene(IntPtr handle) : base(handle){}

		public override void DidMoveToView(SKView view)
		{

			var shelf00 = (SKSpriteNode)this.GetChildNode("shelf00");
			var shelf01 = (SKSpriteNode)this.GetChildNode("shelf01");
			var shelf02 = (SKSpriteNode)this.GetChildNode("shelf02");
			var shelf03 = (SKSpriteNode)this.GetChildNode("shelf03");
			var shelf10 = (SKSpriteNode)this.GetChildNode("shelf10");
			var shelf11 = (SKSpriteNode)this.GetChildNode("shelf11");
			var shelf12 = (SKSpriteNode)this.GetChildNode("shelf12");
			var shelf13 = (SKSpriteNode)this.GetChildNode("shelf13");
			var shelf20 = (SKSpriteNode)this.GetChildNode("shelf20");
			var shelf21 = (SKSpriteNode)this.GetChildNode("shelf21");
			var shelf22 = (SKSpriteNode)this.GetChildNode("shelf22");
			var shelf23 = (SKSpriteNode)this.GetChildNode("shelf23");
			var shelf30 = (SKSpriteNode)this.GetChildNode("shelf30");
			var shelf31 = (SKSpriteNode)this.GetChildNode("shelf31");
			var shelf32 = (SKSpriteNode)this.GetChildNode("shelf32");
			var shelf33 = (SKSpriteNode)this.GetChildNode("shelf33");
			var life1 = (SKSpriteNode)this.GetChildNode("life1");
			var life2 = (SKSpriteNode)this.GetChildNode("life2");
			var life3 = (SKSpriteNode)this.GetChildNode("life3");
			var pauseButton = (SKSpriteNode)this.GetChildNode("pauseButton");

			var levelLabel = (SKLabelNode)this.GetChildNode("levelLabel");

			var pauseFog = (SKSpriteNode)this.GetChildNode("pauseFog");
			var aboutLabel = (SKLabelNode)this.GetChildNode("aboutLabel");
			var aboutLine = (SKSpriteNode)this.GetChildNode("aboutLine");
			var aboutButton = (SKSpriteNode)this.GetChildNode("aboutButton");
			var settingsLabel = (SKLabelNode)this.GetChildNode("settingsLabel");
			var settingsLine = (SKSpriteNode)this.GetChildNode("settingsLine");
			var settingsButton = (SKSpriteNode)this.GetChildNode("settingsButton");
			var resumeLabel = (SKLabelNode)this.GetChildNode("resumeLabel");
			var resumeLine = (SKSpriteNode)this.GetChildNode("resumeLine");
			var resumeButton = (SKSpriteNode)this.GetChildNode("resumeButton");
			var highScoreLabel = (SKLabelNode)this.GetChildNode("highScoreLabel");
			var yourScoreLabel = (SKLabelNode)this.GetChildNode("yourScoreLabel");

			var playLabel = (SKLabelNode)this.GetChildNode("playLabel");
			var playButton = (SKSpriteNode)this.GetChildNode("playButton");


		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			foreach (var touch in touches)
			{
				var location = ((UITouch)touch).LocationInNode(this);


			//	this.GetNodeAtPoint(location);

			}
		}

		public override void Update(double currentTime)
		{
			
		}

		public enterPause() 
		{
			
		}
	}
}
