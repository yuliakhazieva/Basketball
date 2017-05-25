using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using System.Timers;

namespace Basketball
{
	enum SKButtonNodeState
	{
		active, selected, hidden
	}

	//Класс полочки
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
	//Класс кнопки
	public class SKButtonNode : SKSpriteNode
	{

		public delegate void buttDel();
		public event buttDel buttPressed;

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			buttPressed();
		}
	}
	//Класс сцены
	public class GameScene : SKScene, ISKPhysicsContactDelegate
	{
		//Таймеры
		Timer timer1 = new Timer();
		Timer timer2 = new Timer();
		Timer timer3 = new Timer();

		//Спрайты
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
		SKButtonNode backButton;
		SKLabelNode nameLabel1;
		SKLabelNode nameLabel2;
		SKLabelNode rememberLabel1;
		SKLabelNode rememberLabel2;
		SKSpriteNode ball;
		SKLabelNode backLabel;
		SKSpriteNode aboutBackgrnd;
		SKAudioNode timerSound;
		SKButtonNode basShader1;
		SKButtonNode basShader2;
		SKButtonNode basShader3;
		SKButtonNode basShader4;
		SKButtonNode soundOnButton;
		SKSpriteNode settingsBackgrnd;
		SKSpriteNode soundsIndicator;

		//Прочие переменные
		CGVector ballVelocity;
		bool contactHappened = false;
		int userChoice;
		int rightChoice;
		int lives = 3;
		int level = 1;
		int highScore = 1;
		bool newHs = false;
		bool soundOn = true;

		protected GameScene(IntPtr handle) : base(handle) { }

		//Метод вызывается при появлении приложения на экране
		public override void DidMoveToView(SKView view)
		{
			

			timerSound = new SKAudioNode("timer");
			timerSound.Name = "timerSound";
			//Если есть рекорд, то берем его значение
			if (NSUserDefaults.StandardUserDefaults.IntForKey("hs") != 0) 
			{ 
				highScore = (int)NSUserDefaults.StandardUserDefaults.IntForKey("hs");
			}
			timer1.Interval = 1000;
			timer2.Interval = 10000;
			this.PhysicsWorld.Gravity = new CGVector(0, -5);
			Random rand = new Random();

			//Лямбда-выражение, вызывающееся при контакте физических тел с соответствующими масками
			this.PhysicsWorld.DidBeginContact += (object sender, EventArgs e) => 
			{
				pauseButton.UserInteractionEnabled = false;
				//Убеждаемся, что не реагируем на тот же самый контакт дважды
				if (!contactHappened)
				{
					contactHappened = true;
					var contact = sender as SKPhysicsContact;
					//Когда происходит контакт между телами, мы должны учесть, что тело А и тело Б могут меняться от случая к случаю
					if (contact.BodyA.Node.Name == "ball")
					{
						SKSpriteNode ballContact = (SKSpriteNode)contact.BodyA.Node;
						SKSpriteNode basketContact = (SKSpriteNode)contact.BodyB.Node;

						//Запоминаем правильный ответ
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
					//Действия, если пользователь угадал
					if (rightChoice == userChoice)
					{
						level++;
						if (timer2.Interval > 5000)
						{
							timer2.Interval -= 1000;
						}
						if (level > highScore)
						{
							newHs = true;
							highScore = level;
							NSUserDefaults.StandardUserDefaults.SetInt(highScore, "hs");
						}
						rememberLabel1.Text = "Верно \ud83c\udf1f";
						if (soundOn)
						{
							RunAction(SKAction.PlaySoundFileNamed("cheers", false));
						}
					}
					//Действия, если пользователь не угадал
					else
					{
						if (soundOn)
						{
							RunAction(SKAction.PlaySoundFileNamed("whistle", false));
						}
						rememberLabel1.Text = "Неверно \ud83c\udf1a";
						lives--;
						switch (lives)
						{
							case 2:
								{
									life1.Hidden = true;
									break;
								}
							case 1:
								{
									life2.Hidden = true;
									break;
								}
							case 0:
								{
									life3.Hidden = true;
									gameOver();
									break;
								}
						}
					}

					rememberLabel1.RunAction(SKAction.Sequence(SKAction.FadeAlphaTo(1, 2), SKAction.FadeAlphaTo(0, 1)));
					//Прячем полочки
					for (int i = 0; i < 4; i++)
					{
						for (int j = 0; j < 4; j++)
						{
							this.GetChildNode("" + i + j).RunAction(SKAction.FadeAlphaTo(0, 0.5));
						}
					}

					if (lives > 0)
					{
						
						timer3.Interval = 3000;
						timer3.Start();

					}
				}
			};
			//Таймер отсрачивающий новый цикл геймплея
			timer3.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				timer3.Stop();
				timer3.Close();
				playPressed();
			};

			//Таймер отсрачивающий начало геймплея
			timer1.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				if (soundOn )
				{
					this.AddChild(timerSound);
				}
				levelLabel.RunAction(SKAction.FadeAlphaTo(0, 1));
				rememberLabel1.RunAction(SKAction.FadeAlphaTo(0, 1));
				rememberLabel2.RunAction(SKAction.FadeAlphaTo(0, 1));
				//Если это первая игра, то генерируем полочки
				if (this.GetChildNode("00") == null)
				{
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
				}
				//Иначе просто случайно их наклоняем
				else 
				{
					for (int i = 0; i < 4; i++)
					{
						for (int j = 0; j < 4; j++)
						{
							int temp = rand.Next(0, 2);
							bool left = temp == 0 ? true : false;
							if (j == 0) { left = true; } else if (j == 3) { left = false; }
							Shelf sh = this.GetChildNode("" + i + j) as Shelf;
							sh.ZRotation = left ? -10 : 10;
							sh.Alpha = 1;
						}
					}
						
				}
				timer1.Stop();
				timer1.Close();
				timer2.Start();
			};

			//Таймер исчезновения полочек
			timer2.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				if (soundOn )
				{
					timerSound.RemoveFromParent();
				}
				timer2.Stop();
				timer2.Close();

				//Прячем полочки
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						this.GetChildNode("" + i + j).RunAction(SKAction.FadeAlphaTo(0, 0.5));
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
				basShader1.UserInteractionEnabled = true;
				basShader2.UserInteractionEnabled = true;
				basShader3.UserInteractionEnabled = true;
				basShader4.UserInteractionEnabled = true;

				//Ставим мяч на случайную позицию
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

			//Создаем кнопки
			pauseButton = new SKButtonNode();
			pauseButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			pauseButton.Size = SKSpriteNode.FromImageNamed("pause").Size;
			pauseButton.Position = new CGPoint(42, 535); ;
			this.AddChild(pauseButton);

			settingsButton = new SKButtonNode();
			settingsButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			settingsButton.Alpha = (nfloat)0.00001;
			settingsButton.Position = new CGPoint(160, 260);
			settingsButton.Size = new CGSize(320, 58);
			settingsButton.ZPosition = 15;
			this.AddChild(settingsButton);

			soundOnButton = new SKButtonNode();
			soundOnButton.Texture = SKSpriteNode.FromImageNamed("ball").Texture;
			soundOnButton.Alpha = (nfloat)0.00001;
			soundOnButton.Position = new CGPoint(-18, -26);
			soundOnButton.Size = new CGSize(25,41);
			soundOnButton.ZPosition = 202;


			resumeButton = new SKButtonNode();
			resumeButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			resumeButton.Alpha = (nfloat)0.000001;
			resumeButton.Position = new CGPoint(160, 370);
			resumeButton.Size = new CGSize(320, 57);
			this.AddChild(resumeButton);

			aboutButton = new SKButtonNode();
			aboutButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			aboutButton.Alpha = (nfloat)0.0001;
			aboutButton.Position = new CGPoint(160, 20);
			aboutButton.Size = new CGSize(323, 46);
			this.AddChild(aboutButton);
			aboutButton.ZPosition = 10;

			backButton = new SKButtonNode();
			backButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			backButton.Alpha = (nfloat)0.000001;
			backButton.Position = new CGPoint(160, 109);
			backButton.Size = new CGSize(323, 46);
			this.AddChild(backButton);
			backButton.ZPosition = 201;

			playButton = new SKButtonNode();
			playButton.Texture = SKSpriteNode.FromImageNamed("pause").Texture;
			playButton.Position = new CGPoint(160, 203);
			playButton.Size = new CGSize(321, 57);
			this.AddChild(playButton);
			playButton.ZPosition = 20;

			//Создаем корзинки
			basket1 = makeABasket(basket1, 40);
			basket2 = makeABasket(basket1, 120);
			basket3 = makeABasket(basket1, 200);
			basket4 = makeABasket(basket1, 280);

			basShader1 = makeAShader(basShader1, basket1.Position);
			basShader2 = makeAShader(basShader1, basket2.Position);
			basShader3 = makeAShader(basShader1, basket3.Position);
			basShader4 = makeAShader(basShader1, basket4.Position);

			basket1.Name = "basket1";
			basket2.Name = "basket2";
			basket3.Name = "basket3";
			basket4.Name = "basket4";

			//Соединяем переменные со спрайтами на сцене
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
			backLabel = (SKLabelNode)this.GetChildNode("backLabel");
			aboutBackgrnd = (SKSpriteNode)this.GetChildNode("aboutBackgrnd");
			settingsBackgrnd = (SKSpriteNode)this.GetChildNode("settingsBackgrnd");
			soundsIndicator = (SKSpriteNode)settingsBackgrnd.GetChildNode("soundsIndicator");

			ball = SKSpriteNode.FromImageNamed("basketball");
			ball.Name = "ball";
			settingsBackgrnd.AddChild(soundOnButton);

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

			//Подписываем методы на события нажатий кнопок
			pauseButton.buttPressed += pausePressed;
			playButton.buttPressed += playPressed;
			settingsButton.buttPressed += settingsPressed;
			aboutButton.buttPressed += aboutPressed;
			resumeButton.buttPressed += resumePressed;

			backButton.buttPressed += backPressed;
			basShader1.buttPressed += shader1Pressed;
			basShader2.buttPressed += shader2Pressed;
			basShader3.buttPressed += shader3Pressed;
			basShader4.buttPressed += shader4Pressed;

			soundOnButton.buttPressed += soundPressed;

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
			backLabel.Hidden = true;
			aboutBackgrnd.Hidden = true;
			settingsBackgrnd.Hidden = true;

			rememberLabel1.Alpha = 0;
			rememberLabel2.Alpha = 0;

			playButton.UserInteractionEnabled = true;
			pauseButton.UserInteractionEnabled = true;


			settingsButton.UserInteractionEnabled = true;

			//Логика того, что будет оповещать нас о случившемся контакте
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

		//Метод для задания корзинок
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

		public void soundPressed()
		{
			if (soundOn)
			{
				soundOn = false;
				soundsIndicator.RunAction(SKAction.MoveToX(-18, 0.2));
				soundOnButton.Position = new CGPoint(21,-26);
			}
			else
			{
				soundOn = true;
				soundsIndicator.RunAction(SKAction.MoveToX(21, 0.2));
				soundOnButton.Position = new CGPoint(-18,-26);
			}
		}

		public SKButtonNode makeAShader(SKButtonNode shader, CGPoint position)
		{
			shader = new SKButtonNode();
			shader.Color = UIColor.Red;
			shader.Size = basket1.Size;
			shader.Alpha = (nfloat)0.00001;
			shader.Position = position;
			shader.ZPosition = 50;
			this.AddChild(shader);
			shader.UserInteractionEnabled = true;
			return shader;
		}


		//Нажали паузу
		public void pausePressed()
		{
			levelLabel.Hidden = true;
			basShader1.UserInteractionEnabled = false;
			basShader2.UserInteractionEnabled = false;
			basShader3.UserInteractionEnabled = false;
			basShader4.UserInteractionEnabled = false;

			if (soundOn )
			{
				timerSound.RemoveFromParent();
			}
			resumeButton.UserInteractionEnabled = true;
			aboutButton.UserInteractionEnabled = true;
			settingsButton.UserInteractionEnabled = true;
			pauseButton.UserInteractionEnabled = false;
			if (rememberLabel1.Text != "Куда попадет мяч?" && rememberLabel1.Text != "Верно \ud83c\udf1f" && rememberLabel1.Text != "Неверно \ud83c\udf1a")
			{
				timer1.Stop();
				timer2.Stop();
			} 	else 
			{
				//Запоминаем с какой скоростью летел мяч
				ballVelocity = ball.PhysicsBody.Velocity;
				if (this.GetChildNode("00").Alpha != 0)
				{
					ball.PhysicsBody.Pinned = true;
				}
			}
			//Прячем лишние спрайты
			rememberLabel2.Hidden = true;
			rememberLabel1.Hidden = true;
			pauseFog.Alpha = 1;
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
			resumeButton.ZPosition = 100;
			resumeLine.RunAction(SKAction.MoveToX(160,0.5));
			settingsLine.RunAction(SKAction.MoveToX(160,0.5));
			settingsLabel.RunAction(SKAction.MoveToX(160, 0.5));
			resumeLabel.RunAction(SKAction.MoveToX(160,0.5));
			aboutLabel.RunAction(SKAction.MoveToY(15,0.5));
			aboutLine.RunAction(SKAction.MoveToY(32,0.5));
		}

		//Когда-нибудь тут будут настройки
		public void settingsPressed()
		{
			
			resumeLine.Hidden = true;
			resumeLabel.Hidden = true;
			aboutLine.Hidden = true;
			aboutLabel.Hidden = true;
			resumeButton.UserInteractionEnabled = false;
			backButton.UserInteractionEnabled = true;
			backLabel.Hidden = false;
			settingsBackgrnd.Hidden = false;
			soundOnButton.UserInteractionEnabled = true;
		}

		//Кнопка о программе
		public void aboutPressed()
		{
			backButton.UserInteractionEnabled = true;
			backLabel.Hidden = false;
			aboutBackgrnd.Hidden = false;
		}

		//Кнопка назад
		public void backPressed()
		{

			resumeLine.Hidden = false;
			resumeLabel.Hidden = false;
			aboutLine.Hidden = false;
			aboutLabel.Hidden = false;
			aboutBackgrnd.Hidden = true;
			settingsBackgrnd.Hidden = true;
			backLabel.Hidden = true;
			backButton.UserInteractionEnabled = false;
			soundOnButton.UserInteractionEnabled = false;
			resumeButton.UserInteractionEnabled = true;
		}

		//Кнопка продолжить
		public void resumePressed()
		{
			if (rememberLabel1.Text == "Куда попадет мяч?")
			{
				basShader1.UserInteractionEnabled = true;
				basShader2.UserInteractionEnabled = true;
				basShader3.UserInteractionEnabled = true;
				basShader4.UserInteractionEnabled = true;
			}

			settingsButton.UserInteractionEnabled = false;
			rememberLabel2.Hidden = false;
			resumeButton.UserInteractionEnabled = false;
			aboutButton.UserInteractionEnabled = false;
			pauseButton.UserInteractionEnabled = true;

			//Заново генерируем полочки чтобы пользователи не брали паузой доп время на запоминание
			if (rememberLabel1.Text != "Куда попадет мяч?" && rememberLabel1.Text != "Верно \ud83c\udf1f" && rememberLabel1.Text != "Неверно \ud83c\udf1a")
			{
				timer1.Start();
				//timer2.Start();

			}
			else 
			{ 
				ball.PhysicsBody.Velocity = ballVelocity;
				if (this.GetChildNode("00").Alpha != 0)
				{
					ball.PhysicsBody.Pinned = false;
				}
			}

			pauseFog.RunAction(SKAction.FadeAlphaTo(0,0.5));
			resumeLine.RunAction(SKAction.MoveToX(480,0.5));
			settingsLine.RunAction(SKAction.MoveToX(-160,0.5));
			settingsLabel.RunAction(SKAction.MoveToX(-113, 0.5));
			aboutLine.RunAction(SKAction.MoveToX(160,0.5));
			resumeLabel.RunAction(SKAction.MoveToX(467,0.5));
			aboutLabel.RunAction(SKAction.MoveToY(-25,0.5));
			aboutLine.RunAction(SKAction.MoveToY(-3,0.5));

			rememberLabel1.Hidden = false;
		}

		//Начало цикла игры
		public void playPressed()
		{
			pauseButton.UserInteractionEnabled = true;
			pauseButton.Hidden = false;
			yourScoreLabel.Hidden = true;
			highScoreLabel.Hidden = true;
			levelLabel.Alpha = 0;
			levelLabel.Hidden = false;
			//Если до этого пользователь проиграл и хочет начать сначала
			if (lives == 0) 
			{ 
				lives = 3; 

				life1.Hidden = false;
				life2.Hidden = false;
				life3.Hidden = false;

				level = 1;
				timer2.Interval = 10000;
			}
			rememberLabel1.Text = "У вас есть " + timer2.Interval/1000 + " секунд";
			playButton.UserInteractionEnabled = false;
			pauseFog.RunAction(SKAction.FadeAlphaTo(0, 0.5));
			nameLabel1.Hidden = true;
			nameLabel2.Hidden = true;
			playLabel.Hidden = true;
			rememberLabel1.Hidden = false;

			rememberLabel1.RunAction(SKAction.FadeAlphaTo(1, 1));
			rememberLabel2.Hidden = false;
			rememberLabel2.RunAction(SKAction.FadeAlphaTo(1, 1));
			levelLabel.Text = "Уровень " + level;
			levelLabel.RunAction(SKAction.FadeAlphaTo(1, 1));
			timer1.Start();

		}

		//Реакция на нажатие корзинок
		public void shader1Pressed()
		{
			userChoice = 1;
			fall();
		}
        public void shader2Pressed()
		{
			userChoice = 2;
			fall();
		}
	    public void shader3Pressed()
		{
			userChoice = 3;
			fall();
		}
	    public void shader4Pressed()
		{
			userChoice = 4;
			fall();
		}


		//Проигрыш
		public void gameOver()
		{
			rememberLabel1.Text = "Вы проиграли ☹️";
			if (newHs)
			{
				rememberLabel1.Hidden = true;
				highScoreLabel.Text = "НОВЫЙ РЕКОРД \ud83d\udcab: " + highScore;
			} else 
			{
				highScoreLabel.Text = "Рекорд: " + highScore; 
			}

			yourScoreLabel.Text = "Ваш результат: " + level;
			highScoreLabel.Alpha = 0;
			yourScoreLabel.Alpha = 0;

			yourScoreLabel.Hidden = false;
			highScoreLabel.Hidden = false;

			rememberLabel1.RunAction(SKAction.FadeAlphaTo(1,1));
			yourScoreLabel.RunAction(SKAction.FadeAlphaTo(1, 1));
			highScoreLabel.RunAction(SKAction.FadeAlphaTo(1, 1));
			pauseFog.RunAction(SKAction.FadeAlphaTo((nfloat)0.6, 1));
			playLabel.Text = "Заново";
			playLabel.Alpha = 0;
			playLabel.RunAction(SKAction.FadeAlphaTo(1, 1));
			playButton.UserInteractionEnabled = true;
			playLabel.Hidden = false;

		}

		//Падение мяча
		public void fall()
		{
			contactHappened = false;

			basShader1.UserInteractionEnabled = false;
			basShader2.UserInteractionEnabled = false;
			basShader3.UserInteractionEnabled = false;
			basShader4.UserInteractionEnabled = false;

			rememberLabel1.Alpha = 0;

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
