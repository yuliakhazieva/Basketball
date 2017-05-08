using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

namespace Basketball
{
	enum gameState 
	{ 
		welcome, play, pause, tryAgain
	}

	enum MSButtonNodeState
	{
	    active, selected, hidden
	}

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

	public class SKButtonNode : SKSpriteNode
	{
		pulic 
		MSButtonNodeState state = MSButtonNodeState.active;
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			this.Alpha = (System.nfloat)0.5;
		}
		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			
		}
	}

	public class GameScene : SKScene
	{
		SKSpriteNode shelf00;
		SKSpriteNode shelf01;
		SKSpriteNode shelf02;
		SKSpriteNode shelf03;
		SKSpriteNode shelf10;
		SKSpriteNode shelf11;
		SKSpriteNode shelf12;
		SKSpriteNode shelf13;
		SKSpriteNode shelf20;
		SKSpriteNode shelf21;
		SKSpriteNode shelf22;
		SKSpriteNode shelf23;
		SKSpriteNode shelf30;
		SKSpriteNode shelf31;
		SKSpriteNode shelf32;
		SKSpriteNode shelf33;
		SKSpriteNode life1;
		SKSpriteNode life2;
		SKSpriteNode life3;
		SKSpriteNode pauseButton;
		SKLabelNode levelLabel;
		SKSpriteNode pauseFog;
		SKLabelNode aboutLabel;
		SKSpriteNode aboutLine;
		SKSpriteNode aboutButton;
		SKLabelNode settingsLabel;
		SKSpriteNode settingsLine;
		SKSpriteNode settingsButton;
		SKLabelNode resumeLabel;
		SKSpriteNode resumeLine;
		SKSpriteNode resumeButton;
		SKLabelNode highScoreLabel;
		SKLabelNode yourScoreLabel;
		SKLabelNode playLabel;
		SKSpriteNode playButton;

		gameState thisGame;
		protected GameScene(IntPtr handle) : base(handle){}

		public override void DidMoveToView(SKView view)
		{

			shelf00 = (SKSpriteNode)this.GetChildNode("shelf00");
			shelf01 = (SKSpriteNode)this.GetChildNode("shelf01");
			shelf02 = (SKSpriteNode)this.GetChildNode("shelf02");
			shelf03 = (SKSpriteNode)this.GetChildNode("shelf03");
			shelf10 = (SKSpriteNode)this.GetChildNode("shelf10");
			shelf11 = (SKSpriteNode)this.GetChildNode("shelf11");
			shelf12 = (SKSpriteNode)this.GetChildNode("shelf12");
			shelf13 = (SKSpriteNode)this.GetChildNode("shelf13");
			shelf20 = (SKSpriteNode)this.GetChildNode("shelf20");
			shelf21 = (SKSpriteNode)this.GetChildNode("shelf21");
			shelf22 = (SKSpriteNode)this.GetChildNode("shelf22");
			shelf23 = (SKSpriteNode)this.GetChildNode("shelf23");
			shelf30 = (SKSpriteNode)this.GetChildNode("shelf30");
			shelf31 = (SKSpriteNode)this.GetChildNode("shelf31");
			shelf32 = (SKSpriteNode)this.GetChildNode("shelf32");
			shelf33 = (SKSpriteNode)this.GetChildNode("shelf33");
			life1 = (SKSpriteNode)this.GetChildNode("life1");
			life2 = (SKSpriteNode)this.GetChildNode("life2");
			life3 = (SKSpriteNode)this.GetChildNode("life3");
			pauseButton = (SKSpriteNode)this.GetChildNode("pauseButton");

			levelLabel = (SKLabelNode)this.GetChildNode("levelLabel");

			pauseFog = (SKSpriteNode)this.GetChildNode("pauseFog");
			aboutLabel = (SKLabelNode)this.GetChildNode("aboutLabel");
			aboutLine = (SKSpriteNode)this.GetChildNode("aboutLine");
			aboutButton = (SKSpriteNode)this.GetChildNode("aboutButton");
			settingsLabel = (SKLabelNode)this.GetChildNode("settingsLabel");
			settingsLine = (SKSpriteNode)this.GetChildNode("settingsLine");
			settingsButton = (SKSpriteNode)this.GetChildNode("settingsButton");
			resumeLabel = (SKLabelNode)this.GetChildNode("resumeLabel");
			resumeLine = (SKSpriteNode)this.GetChildNode("resumeLine");
			resumeButton = (SKSpriteNode)this.GetChildNode("resumeButton");
			highScoreLabel = (SKLabelNode)this.GetChildNode("highScoreLabel");
			yourScoreLabel = (SKLabelNode)this.GetChildNode("yourScoreLabel");

			playLabel = (SKLabelNode)this.GetChildNode("playLabel");
			playButton = (SKSpriteNode)this.GetChildNode("playButton");


		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			foreach (var touch in touches)
			{
				var location = ((UITouch)touch).LocationInNode(this);
				SKNode touchedNode = this.GetNodeAtPoint(location);

			}
		}

		public override void Update(double currentTime)
		{
			
		}

		public void enterPause() 
		{
			pauseFog.Hidden = false;
			aboutLabel.Hidden = false;
			aboutLine.Hidden = false;
			aboutButton.Hidden = false;
			settingsLabel.Hidden = false;
			settingsLine.Hidden = false;
			settingsButton.Hidden = false;
			resumeLabel.Hidden = false;
			resumeLine.Hidden = false;
			resumeButton.Hidden = false;
			highScoreLabel.Hidden = false;
			yourScoreLabel.Hidden = false;
		}
	}
}
