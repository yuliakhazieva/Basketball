using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using System.Timers;

namespace Basketball
{
	enum gameState
	{
		welcome, play, pause, tryAgain
	}

	enum SKButtonNodeState
	{
		active, selected, hidden
	}

	public class Shelf : SKSpriteNode
	{
		public bool isLeft = false;

		public Shelf(bool left, CGPoint point)
		{
			isLeft = left;
			this.Position = point;
			this.Texture = FromImageNamed("shelf").Texture;
			this.Size = FromImageNamed("shelf").Texture.Size;
			this.ZRotation = isLeft ? -10 : 10;
		}

	}

	public class SKButtonNode : SKSpriteNode
	{

		public delegate void buttDel();

		public event buttDel buttPressed;
		public event buttDel buttTouched;

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			// buttTouched();
		}
		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			buttPressed();
		}
	}

	public class GameScene : SKScene, ISKPhysicsContactDelegate
	{
		Timer timer1 = new Timer();
		Timer timer2 = new Timer();
		SKSpriteNode life1;
		SKSpriteNode life2;
		SKSpriteNode life3;
		SKButtonNode pauseButton;
		SKLabelNode levelLabel;
		SKSpriteNode pauseFog;
		SKLabelNode aboutLabel;
		SKSpriteNode aboutLine;
		SKButtonNode aboutButton;
		SKLabelNode settingsLabel;
		SKSpriteNode settingsLine;
		SKButtonNode settingsButton;
		SKLabelNode resumeLabel;
		SKSpriteNode resumeLine;
		SKButtonNode resumeButton;
		SKLabelNode highScoreLabel;
		SKLabelNode yourScoreLabel;
		SKLabelNode playLabel;
		SKButtonNode playButton;
		SKButtonNode basket1;
		SKButtonNode basket2;
		SKButtonNode basket3;
		SKButtonNode basket4;
		SKLabelNode nameLabel1;
		SKLabelNode nameLabel2;
		SKLabelNode rememberLabel1;
		SKLabelNode rememberLabel2;
		SKSpriteNode ball;
		int userChoice;
		int rightChoice;

		gameState thisGame;
		protected GameScene(IntPtr handle) : base(handle) { }

		public override void DidMoveToView(SKView view)
		{

			timer1.Interval = 1000;
			timer2.Interval = 10000;
			this.PhysicsWorld.Gravity = new CGVector(0, -5);
			Random rand = new Random();
			//this.PhysicsWorld.ContactDelegate = this;
			this.PhysicsWorld.DidBeginContact += (object sender, EventArgs e) => 
			{
				var contact = sender as SKPhysicsContact;
				if (contact.BodyA.Node.Name == "ball")
				{
					SKSpriteNode ballContact = (SKSpriteNode)contact.BodyA.Node;
					SKSpriteNode basketContact = (SKSpriteNode)contact.BodyB.Node;

					switch (basketContact.Name)
					{
						case "basket1":
						rightChoice = 1;
						break;
						case "basket2":
						rightChoice = 2;
						break;
						case "basket3":
						rightChoice = 3;
						break;
						case "basket4":
						rightChoice = 4;
						break;
					}
					
				}
				else 
				{
					SKSpriteNode ballContact = (SKSpriteNode)contact.BodyB.Node;
					SKSpriteNode basketContact = (SKSpriteNode)contact.BodyA.Node;	

					switch (basketContact.Name)
					{
						case "basket1":
						rightChoice = 1;
						break;
						case "basket2":
						rightChoice = 2;
						break;
						case "basket3":
						rightChoice = 3;
						break;
						case "basket4":
						rightChoice = 4;
						break;
					}
				}

				ball.RunAction(SKAction.Sequence(SKAction.RotateByAngle(10, 1), SKAction.RemoveFromParent()));
			};


			timer1.Elapsed += (object sender, ElapsedEventArgs e) =>
			{

				rememberLabel1.RunAction(SKAction.FadeAlphaTo(0, 1));
				rememberLabel2.RunAction(SKAction.FadeAlphaTo(0, 1));
				Shelf[][] shelfArray = new Shelf[4][];
				for (int i = 0; i < shelfArray.Length; ++i)
				{
					shelfArray[i] = new Shelf[4];
					for (int j = 0; j < shelfArray[i].Length; ++j)
					{
						CGPoint shelfPlace = new CGPoint(40 + 80 * j, 400 - 90 * i);
						int temp = rand.Next(0, 2);
						bool left = temp == 0 ? true : false;
						if (j == 0) { left = true; } else if (j == 3) { left = false; }
						shelfArray[i][j] = new Shelf(left, shelfPlace);
						shelfArray[i][j].PhysicsBody = SKPhysicsBody.CreateRectangularBody(shelfArray[i][j].Size);
						shelfArray[i][j].PhysicsBody.Dynamic = false;
						shelfArray[i][j].PhysicsBody.Friction = (nfloat)0.1;
						shelfArray[i][j].PhysicsBody.ContactTestBitMask = 0;
						shelfArray[i][j].PhysicsBody.CategoryBitMask = 8;

						shelfArray[i][j].Name = "" + i + j;
						this.AddChild(shelfArray[i][j]);
					}

				}
				timer1.Stop();
				timer1.Close();
				timer2.Start();
			};
			timer2.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				timer2.Stop();
				timer2.Close();
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						this.GetChildNode("" + i + j).RunAction(SKAction.FadeAlphaTo(0, 1));
					}
				}
				rememberLabel1.Text = "Куда попадет мяч?";
				rememberLabel1.RunAction(SKAction.FadeAlphaTo(1, 1));
				int ballCol = rand.Next(0, 4);

				SKAction rotateBackAndForth = SKAction.Sequence(SKAction.RotateToAngle((nfloat)6.0, 0.5), SKAction.RotateToAngle((nfloat)6.5, 0.5));
				basket1.RunAction(SKAction.RepeatActionForever(rotateBackAndForth));
				basket2.RunAction(SKAction.RepeatActionForever(rotateBackAndForth));
				basket3.RunAction(SKAction.RepeatActionForever(rotateBackAndForth));
				basket4.RunAction(SKAction.RepeatActionForever(rotateBackAndForth));


				ball.Position = new CGPoint(40 + ballCol * 80, 500);
				ball.PhysicsBody = SKPhysicsBody.CreateCircularBody(5);
				ball.PhysicsBody.ContactTestBitMask = 2;
				ball.PhysicsBody.CollisionBitMask = 12;
				ball.PhysicsBody.CategoryBitMask = 1;
				ball.PhysicsBody.Pinned = true;
				ball.XScale = (nfloat)0.5;
				ball.YScale = (nfloat)0.5;
				ball.PhysicsBody.LinearDamping = 0;
				AddChild(ball);

			};
			life1 = (SKSpriteNode)this.GetChildNode("life1");
			life2 = (SKSpriteNode)this.GetChildNode("life2");
			life3 = (SKSpriteNode)this.GetChildNode("life3");

			pauseButton = new SKButtonNode();
			pauseButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			pauseButton.Size = SKSpriteNode.FromImageNamed("pause").Size;
			pauseButton.Position = new CGPoint(42, 535); ;
			this.AddChild(pauseButton);

			settingsButton = new SKButtonNode();
			settingsButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			settingsButton.Alpha = 0;
			settingsButton.Position = new CGPoint(160, 260);
			settingsButton.Size = new CGSize(320, 58);
			this.AddChild(settingsButton);

			resumeButton = new SKButtonNode();
			resumeButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			resumeButton.Alpha = 0;
			resumeButton.Position = new CGPoint(160, 370);
			resumeButton.Size = new CGSize(320, 57);
			this.AddChild(resumeButton);

			aboutButton = new SKButtonNode();
			aboutButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			aboutButton.Alpha = 0;
			aboutButton.Position = new CGPoint(160, 20);
			aboutButton.Size = new CGSize(323, 46);
			this.AddChild(aboutButton);

			playButton = new SKButtonNode();
			playButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			playButton.Alpha = 0;
			playButton.Position = new CGPoint(160, 203);
			playButton.Size = new CGSize(321, 57);
			this.AddChild(playButton);
			playButton.ZPosition = 20;

			basket1 = makeABasket(basket1, 40);
			basket2 = makeABasket(basket1, 120);
			basket3 = makeABasket(basket1, 200);
			basket4 = makeABasket(basket1, 280);

			levelLabel = (SKLabelNode)this.GetChildNode("levelLabel");

			pauseFog = (SKSpriteNode)this.GetChildNode("pauseFog");
			aboutLabel = (SKLabelNode)this.GetChildNode("aboutLabel");
			aboutLine = (SKSpriteNode)this.GetChildNode("aboutLine");
			settingsLabel = (SKLabelNode)this.GetChildNode("settingsLabel");
			settingsLine = (SKSpriteNode)this.GetChildNode("settingsLine");
			resumeLabel = (SKLabelNode)this.GetChildNode("resumeLabel");
			resumeLine = (SKSpriteNode)this.GetChildNode("resumeLine");
			highScoreLabel = (SKLabelNode)this.GetChildNode("highScoreLabel");
			yourScoreLabel = (SKLabelNode)this.GetChildNode("yourScoreLabel");
			playLabel = (SKLabelNode)this.GetChildNode("playLabel");
			nameLabel1 = (SKLabelNode)this.GetChildNode("nameLabel1");
			nameLabel2 = (SKLabelNode)this.GetChildNode("nameLabel2");
			rememberLabel1 = (SKLabelNode)this.GetChildNode("rememberLabel1");
			rememberLabel2 = (SKLabelNode)this.GetChildNode("rememberLabel2");
			ball = SKSpriteNode.FromImageNamed("basketball");
			ball.Name = "ball";
			basket1.PhysicsBody = SKPhysicsBody.Create(basket1.Texture, basket1.Size);
			basket2.PhysicsBody = SKPhysicsBody.Create(basket1.Texture, basket1.Size);
			basket3.PhysicsBody = SKPhysicsBody.Create(basket1.Texture, basket1.Size);
			basket4.PhysicsBody = SKPhysicsBody.Create(basket1.Texture, basket1.Size);



			basket1.PhysicsBody.Dynamic = false;
			basket2.PhysicsBody.Dynamic = false;
			basket3.PhysicsBody.Dynamic = false;
			basket4.PhysicsBody.Dynamic = false;

			basket1.PhysicsBody.CollisionBitMask = 00000000;
			basket2.PhysicsBody.CollisionBitMask = 00000000;
			basket3.PhysicsBody.CollisionBitMask = 00000000;
			basket4.PhysicsBody.CollisionBitMask = 00000000;

			pauseButton.buttPressed += pausePressed;
			playButton.buttPressed += playPressed;
			settingsButton.buttPressed += settingsPressed;
			aboutButton.buttPressed += aboutPressed;
			basket1.buttPressed += basket1Pressed;
			basket2.buttPressed += basket2Pressed;
			basket3.buttPressed += basket3Pressed;
			basket4.buttPressed += basket4Pressed;

			pauseButton.Hidden = true;
			settingsLine.Hidden = true;
			resumeLine.Hidden = true;
			aboutLine.Hidden = true;
			aboutLabel.Hidden = true;
			settingsLabel.Hidden = true;
			resumeLabel.Hidden = true;
			yourScoreLabel.Hidden = true;
			highScoreLabel.Hidden = true;
			levelLabel.Hidden = true;
			rememberLabel1.Hidden = true;
			rememberLabel2.Hidden = true;

			rememberLabel1.Alpha = 0;
			rememberLabel2.Alpha = 0;

			playButton.UserInteractionEnabled = true;
			pauseButton.UserInteractionEnabled = true;
			resumeButton.UserInteractionEnabled = true;
			aboutButton.UserInteractionEnabled = true;
			settingsButton.UserInteractionEnabled = true;
			basket1.UserInteractionEnabled = true;
			basket2.UserInteractionEnabled = true;
			basket3.UserInteractionEnabled = true;
			basket4.UserInteractionEnabled = true;

			basket1.PhysicsBody.ContactTestBitMask = 00000001;
			basket2.PhysicsBody.ContactTestBitMask = 00000001;
			basket3.PhysicsBody.ContactTestBitMask = 00000001;
			basket4.PhysicsBody.ContactTestBitMask = 00000001;

			basket1.PhysicsBody.CategoryBitMask = 2;
			basket2.PhysicsBody.CategoryBitMask = 2;
			basket3.PhysicsBody.CategoryBitMask = 2;
			basket4.PhysicsBody.CategoryBitMask = 2;


			playButton.Alpha = (float)0.000001;
		}


		public SKButtonNode makeABasket(SKButtonNode basket, int xCoord)
		{
			basket = new SKButtonNode();
			basket.Texture = SKTexture.FromImageNamed("basket");
			basket.Size = SKSpriteNode.FromImageNamed("basket").Size;
			basket.XScale = (nfloat)0.3;
			basket.YScale = (nfloat)0.3;
			basket.Position = new CGPoint(xCoord, 39);
			this.AddChild(basket);
			return basket;
		}
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			foreach (var touch in touches)
			{

			}
		}

		public override void Update(double currentTime)
		{

		}

		public void pausePressed()
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

		public void settingsPressed()
		{

		}

		public void aboutPressed()
		{

		}

		public void resumePressed()
		{

		}

		public void playPressed()
		{
			playButton.RemoveFromParent();
			pauseFog.Hidden = true;
			nameLabel1.Hidden = true;
			nameLabel2.Hidden = true;
			playLabel.Hidden = true;
			rememberLabel1.Hidden = false;

			rememberLabel1.RunAction(SKAction.FadeAlphaTo(1, 1));
			rememberLabel2.Hidden = false;
			rememberLabel2.RunAction(SKAction.FadeAlphaTo(1, 1));

			timer1.Start();

		}

		public void basket1Pressed()
		{
			userChoice = 1;
			fall();
		}

		public void basket2Pressed()
		{
			userChoice = 2;
			fall();
		}

		public void basket3Pressed()
		{
			userChoice = 3;
			fall();
		}

		public void basket4Pressed()
		{
			userChoice = 4;
			fall();
		}

		public void fall()
		{

			basket1.RemoveAllActions();
			basket2.RemoveAllActions();
			basket3.RemoveAllActions();
			basket4.RemoveAllActions();

			basket1.ZRotation = 0;
			basket2.ZRotation = 0;
			basket3.ZRotation = 0;
			basket4.ZRotation = 0;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					this.GetChildNode("" + i + j).RunAction(SKAction.FadeAlphaTo(1, 1));
				}
			}
			ball.PhysicsBody.Pinned = false;

		}


	}
}
