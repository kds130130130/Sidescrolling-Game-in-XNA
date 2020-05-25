using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KeepGrinding
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D p1color, p2color;
        int p1LastSkin, p2LastSkin;
        Texture2D p1Label, p2Label;
        Texture2D p1Suffering, p2Suffering;
        Texture2D playerOneRightTilt, playerOneLeftTilt, playerOneNoTilt;
        Texture2D playerTwoRightTilt, playerTwoLeftTilt, playerTwoNoTilt;
        Texture2D p1Punching, p2Punching;
        int p1SufferingTimer, p2SufferingTimer;
        double p1Knockback, p2Knockback;
        Texture2D allocBackground, combatBackground;
        Texture2D allocFloorTexture, combatFloorTexture;
        Vector2 p1LabelPos, p2LabelPos;
        Vector2 floorPos;
        Model player1model;
        Model player2model;
        Model weapon1model;
        Model weapon2model;
        Texture2D w1color, w2color;
        Player[] player = new Player[2];
        Vector3 thirdPersonReference = new Vector3(100, 0, 0);
        float avatarYaw = MathHelper.PiOver2;
        Matrix rotationMatrix;
        Vector3 transformedReference;
        Vector3 avatarPosition = new Vector3(2.5f, 0, 0);
        Vector3 cameraPosition;
        Vector3 p1location, p2location, w1location, w2location;
        float p1position, p2position, w1position, w2position;
        bool punch1, punch2, punch1Animation, punch2Animation, punch1out, punch2out;
        bool p1IsWalkingLeft, p2IsWalkingLeft, p1IsWalkingRight, p2IsWalkingRight;
        float SPEED_DIVISOR = 150f;
        float PUNCH_LENGTH = 3f;
        int p1WalkAnimationFrame, p2WalkAnimationFrame;
        int p1WalkingAnimationTimer, p2WalkingAnimationTimer;

        Vector2 fontPos;
        SpriteFont font;
        Vector2 FontOrigin;
        String output;
        bool gameIsOver;
        bool allocatingStatsStage;
        int rand;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            floorPos = new Vector2(0, 340);
            p1Knockback = p2Knockback = 0;
            p1WalkAnimationFrame = p2WalkAnimationFrame = 0;
            p1WalkingAnimationTimer = p2WalkingAnimationTimer = 0;
            p1IsWalkingLeft = p2IsWalkingLeft = p1IsWalkingRight = p2IsWalkingRight = false;

            // TODO: Add your initialization logic here
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
            // mapScale = 1.5;
            // AllocConsole();
            IsMouseVisible = true;
            player[0] = new Player();
            //player[0].setStats(5000, 33, 30, 33);
            player[1] = new Player();
            //player[1].setStats(5000, 33, 1, 33);
            p1position = w1position = 0;
            p2position = w2position = 5;
            p1location = new Vector3(p1position, 0, 0);//NOTE: 3D instead of 2D for a reason?
            p2location = new Vector3(p2position, 0, 0);
            punch1 = punch2 = punch1Animation = punch2Animation = punch1out = punch2out = gameIsOver = false;
            allocatingStatsStage = true;
            fontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 4);
            base.Initialize();
        }

        public void calculateDamage(int defender)
        {
            if (defender == 1)
            {
                player[1].takeDamage(player[0].getSpeed() * player[0].getAttack() / player[1].getDefense());
            }
            else if (defender == 0)
            {
                player[0].takeDamage(player[1].getSpeed() * player[1].getAttack() / player[0].getDefense());
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p2color = Content.Load<Texture2D>("p2Standing");
            p1color = Content.Load<Texture2D>("p1Standing");
            /*white = Content.Load<Texture2D>("Marble");
            stripes = Content.Load<Texture2D>("Scratches");
            brown = Content.Load<Texture2D>("Wood");*/

            allocBackground = Content.Load<Texture2D>("Blue sky");
            allocFloorTexture = Content.Load<Texture2D>("Green field");
            combatBackground = Content.Load<Texture2D>("Blue sky");
            combatFloorTexture = Content.Load<Texture2D>("Green field");

            p1Suffering = Content.Load<Texture2D>("image6");
            p2Suffering = Content.Load<Texture2D>("image6_2");
            p1Label = Content.Load<Texture2D>("Player one");
            p2Label = Content.Load<Texture2D>("Player two");

            playerOneRightTilt = Content.Load<Texture2D>("image4");
            playerOneLeftTilt = Content.Load<Texture2D>("image2");
            playerOneNoTilt = Content.Load<Texture2D>("p1Standing");

            playerTwoRightTilt = Content.Load<Texture2D>("image2_2");
            playerTwoLeftTilt = Content.Load<Texture2D>("image4_2");
            playerTwoNoTilt = Content.Load<Texture2D>("p2Standing");

            p1Punching = Content.Load<Texture2D>("image5");
            p2Punching = Content.Load<Texture2D>("image5_2");

            w1color = Content.Load<Texture2D>("image7");
            w2color = Content.Load<Texture2D>("image7_2");

            player1model = Content.Load<Model>("Cube");
            player2model = Content.Load<Model>("Cube");
            weapon1model = Content.Load<Model>("Cube");
            weapon2model = Content.Load<Model>("Cube");
            font = Content.Load<SpriteFont>("CourierNew");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //punch 1
            if (allocatingStatsStage)
            {
                output = "Player 2                                         Player 1\n\n";
                output += "     Press and Hold the Corresponding Buttons to Add\n     Press (Space) or Spend All Points to Begin\n\n";
                output += "Points Remaining: " + player[1].getPoints();
                if (player[1].getPoints() >= 100)
                {
                    output += " ";
                }
                for (int i = output.Length; i < 201; i++)//201
                {
                    output += " ";
                }
                output += "Points Remaining: " + player[0].getPoints();
                if (player[0].getPoints() < 100)
                {
                    output += " ";
                }
                /*for (int i = output.Length; i < 185; i++)//185
                {
                    output += " ";
                }*/
                output += "\n      (A) Attack: " + (int)player[1].getAttack();
                for (int i = output.Length; i < 263; i++)//265
                {
                    output += " ";
                }
                output += "(Left) Attack: " + (int)player[0].getAttack();
                for (int i = output.Length; i < 258; i++)
                {
                    output += " ";
                }
                output += "\n     (D) Defense: " + (int)player[1].getDefense();
                for (int i = output.Length; i < 320; i++)//309
                {
                    output += " ";
                }
                if (player[0].getAttack() >= 10)
                {
                    output += " ";
                }
                output += "(Right) Defense: " + (int)player[0].getDefense();
                for (int i = output.Length; i < 331; i++)
                {
                    output += " ";
                }
                output += "\n       (W) Speed: " + (int)player[1].getSpeed();
                for (int i = output.Length; i < 379; i++)//383
                {
                    output += " ";
                }
                if (player[0].getAttack() >= 10)
                {
                    output += " ";
                }
                if (player[0].getDefense() >= 10)
                {
                    output += " ";
                }
                output += "(Up) Speed: " + (int)player[0].getSpeed();
                

                //adding stats
                //player 2
                if (player[1].getPoints() > 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        player[1].addAttack(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        player[1].addDefense(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        player[1].addSpeed(0.1f);
                    }
                }
                //player 1
                if (player[0].getPoints() > 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        player[0].addAttack(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        player[0].addDefense(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        player[0].addSpeed(0.1f);
                    }
                }
                //quick start
                if (Keyboard.GetState().IsKeyDown(Keys.Space))//Starts game. Change on-screen text if the hotkey changes
                {
                    allocatingStatsStage = false;
                }
                //ending the stage
                if (player[0].getPoints() <= 0 && player[1].getPoints() <= 0 && Keyboard.GetState().GetPressedKeys().Length == 0)
                {
                    allocatingStatsStage = false;
                }

            } // end allocating stats stage
            else // start fighting stage
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    punch1 = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && punch1 == false)
                {
                    punch1 = true;
                }
                if (punch1 == true && punch1Animation == false)
                {
                    punch1Animation = punch1out = true;
                }
                if (punch1Animation == true)
                {
                    if (punch1out == true)
                    {
                        w1position += player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w1position, p1position) > PUNCH_LENGTH)
                    {
                        punch1out = false;
                    }
                    if (punch1out == false)
                    {
                        w1position -= player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w1position, p1position) < 0.2 && punch1out == false)
                    {
                        w1position = p1position;
                        punch1Animation = false;
                    }
                }
                //punch 2
                if (Keyboard.GetState().IsKeyUp(Keys.W))
                {
                    punch2 = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && punch2 == false)
                {
                    punch2 = true;
                }
                if (punch2 == true && punch2Animation == false)
                {
                    punch2Animation = punch2out = true;
                }
                if (punch2Animation == true)
                {
                    if (punch2out == true)
                    {
                        w2position -= player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w2position, p2position) > PUNCH_LENGTH)
                    {
                        punch2out = false;
                    }
                    if (punch2out == false)
                    {
                        w2position += player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w2position, p2position) < 0.2 && punch2out == false)
                    {
                        w2position = p2position;
                        punch2Animation = false;
                    }
                }
                //player 2 movement
                if (punch2Animation == false && Keyboard.GetState().IsKeyDown(Keys.W) == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A) && p2position < 11)
                    {
                        p2IsWalkingLeft = true;
                        p2position += player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    else
                    {
                        p2IsWalkingLeft = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D) && p2position > p1position + 1.6f)
                    {
                        p2IsWalkingRight = true;
                        p2position -= player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    else
                    {
                        p2IsWalkingRight = false;
                    }
                    w2position = p2position;
                }
                //player 1 movement
                if (punch1Animation == false && Keyboard.GetState().IsKeyDown(Keys.Up) == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) && p1position < p2position - 1.6f)
                    {
                        p1position += player[0].getSpeed() / SPEED_DIVISOR;
                        p1IsWalkingLeft = true;
                    }
                    else
                    {
                        p1IsWalkingLeft = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) && p1position > -6)
                    {
                        p1position -= player[0].getSpeed() / SPEED_DIVISOR;
                        p1IsWalkingRight = true;
                    }
                    else
                    {
                        p1IsWalkingRight = false;
                    }
                    w1position = p1position;
                }
                // damage from punches
                if (gameIsOver == false)
                {
                    if (w1position > (p2position - 1))
                    {
                        calculateDamage(1);
                        p2ShowSuffering(50);
                        if (p2position > 11)
                        {
                            p2Knockback = 0;
                        }
                        else
                        {
                            p2Knockback = 0.5;//NOTE: change this to change initial knockback force
                        }
                    }
                    if (w2position < (p1position + 1))
                    {
                        calculateDamage(0);
                        p1ShowSuffering(50);
                        p1Knockback = 0.5;
                        if (p1position < -6)
                        {
                            p1Knockback = 0;
                        }
                        else
                        {
                            p1Knockback = 0.5;//NOTE: change this to change initial knockback force
                        }
                    }
                }
                // TODO: Add your update logic here

                

                //Momentum from punches
                if (p1Knockback > 0) 
                {
                    p1position -= (float)p1Knockback;
                    
                    if (p1position < -6)
                    {
                        p1Knockback = 0;
                    }
                    else
                    {
                        p1Knockback -= 0.05;//NOTE: change this to modify sliding distance
                    }
                }

                if (p2Knockback > 0)
                {
                    p2position += (float)p2Knockback;
                    //p2Knockback-=0.05;

                    if (p2position > 11)
                    {
                        p2Knockback = 0;
                    }
                    else
                    {
                        p2Knockback -= 0.05;//NOTE: change this to modify sliding distance
                    }
                }

                //Walking animation
                if (p1IsWalkingLeft || p1IsWalkingRight)
                {
                    p1WalkingAnimationTimer++;
                    if (p1WalkingAnimationTimer == 20)
                    {
                        p1WalkAnimationFrame++;
                        p1WalkingAnimationTimer = 0;
                        if (p1WalkAnimationFrame == 4)
                        {
                            p1WalkAnimationFrame = 0;
                        }
                    }
                }
                else

                {
                    p1WalkAnimationFrame = 0;
                }
                p1color = p1WalkingSkin(p1WalkAnimationFrame);

                if (p2IsWalkingLeft || p2IsWalkingRight)
                {
                    p2WalkingAnimationTimer++;
                    if (p2WalkingAnimationTimer == 20)
                    {
                        p2WalkAnimationFrame++;
                        p2WalkingAnimationTimer = 0;
                        if (p2WalkAnimationFrame == 4)
                        {
                            p2WalkAnimationFrame = 0;
                        }
                    }
                }
                else
                {
                    p2WalkAnimationFrame = 0;
                }
                p2color = p2WalkingSkin(p2WalkAnimationFrame);


                //Animation of characters suffering
                //Triggered by p1ShowSuffering methods
                if (p1SufferingTimer > 0)
                {
                    //Sets new texture
                    p1color = p1Suffering;
                    p1SufferingTimer -= 1;
                }
                else
                {
                    //Replaces texture with old one
                    //p1color = p1NewSkin(p1LastSkin);
                }
                if (p2SufferingTimer > 0)
                {
                    //Sets new texture
                    p2color = p2Suffering;
                    p2SufferingTimer -= 1;
                }
                else
                {
                    //Replaces texture with old one
                    //p2color = p2NewSkin(p2LastSkin);

                }

                //punching animation
                if (punch1Animation)
                {
                    p1color = p1Punching;
                }

                if (punch2Animation)
                {
                    p2color = p2Punching;
                }

                p1location = new Vector3(p1position, 0, 0);
                p2location = new Vector3(p2position, 0, 0);
                w1location = new Vector3(w1position, 0, 0);
                w2location = new Vector3(w2position, 0, 0);
                output = "Player 2 HP: " + player[1].getHealth();
                output += "                            ";
                output += "Player 1 HP: " + player[0].getHealth();
                //output += "\n" + p2GetScreenX() + "    " + p1GetScreenX();//Displays screen coordinates of getScreenX method
                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0)
                {
                    gameOver(2);
                }
                else if (player[0].getHealth() <= 0)
                {
                    gameOver(1);
                }
                else if (player[1].getHealth() <= 0)
                {
                    gameOver(0);
                }
            } // end fighting stage
            // changing skins
            /*if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                p2color = p2NewSkin(new Random().Next(0,5));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                p1color = p1NewSkin(new Random().Next(0,5));
            }*/

            base.Update(gameTime);
        }

        /*Texture2D newSkin()
        {
            rand = new Random().Next(0, 5);
            switch (rand)
            {
                case 0: return blue;
                case 1: return white;
                case 2: return stripes;
                case 3: return brown;
                default: return grey;
            }
        }*/

        Texture2D p1WalkingSkin(int frame)
        {
            switch (frame)
            {
                case 0: return playerOneNoTilt;
                case 1: return playerOneLeftTilt;
                case 2: return playerOneNoTilt;
                case 3: return playerOneRightTilt;
                default: return playerOneNoTilt;
            }
        }

        Texture2D p2WalkingSkin(int frame)
        {
            switch (frame)
            {
                case 0: return playerTwoNoTilt;
                case 1: return playerTwoLeftTilt;
                case 2: return playerTwoNoTilt;
                case 3: return playerTwoRightTilt;
                default: return playerTwoNoTilt;
            }
        }

        /*Texture2D p1NewSkin(int skinID)
        {
            p1LastSkin = skinID;
            switch (skinID)
            {
                case 0: return blue;
                case 1: return white;
                case 2: return stripes;
                case 3: return brown;
                default: return grey;
            }
        }*/

        /*Texture2D p2NewSkin(int skinID)
        {
            p2LastSkin = skinID;
            switch (skinID)
            {
                case 0: return blue;
                case 1: return white;
                case 2: return stripes;
                case 3: return brown;
                default: return grey;
            }
        }*/

        void p1ShowSuffering(int duration)
        {
            p1SufferingTimer = duration;
        }

        void p2ShowSuffering(int duration)
        {
            p2SufferingTimer = duration;
        }

        //Converts 3D coordinates of players to return screen coordinates (very crudely)
        int p1GetScreenX()
        {
            Vector3 coord = GraphicsDevice.Viewport.Project(p1location, Matrix.CreatePerspectiveFieldOfView(0.1f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f), Matrix.CreateLookAt(cameraPosition, avatarPosition, new Vector3(0.0f, 0.0f, 1.0f)), Matrix.CreateScale(new Vector3(1,1,1)) * Matrix.CreateFromYawPitchRoll(0, 0, 0 /*rotation.(x,y,z)*/) * Matrix.CreateTranslation(p1location));
            float fValue = (coord.X + 689.8211f) * (1200f/2250.2361f);
            int retVal = (int)Math.Round(fValue, 0);
            return retVal;
        }

        int p2GetScreenX()
        {
            Vector3 coord = GraphicsDevice.Viewport.Project(p2location, Matrix.CreatePerspectiveFieldOfView(0.1f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f), Matrix.CreateLookAt(cameraPosition, avatarPosition, new Vector3(0.0f, 0.0f, 1.0f)), Matrix.CreateScale(new Vector3(1, 1, 1)) * Matrix.CreateFromYawPitchRoll(0, 0, 0 /*rotation.(x,y,z)*/) * Matrix.CreateTranslation(p2location));
            float fValue = (coord.X + 689.8211f) * (1200f / 2250.2361f);
            int retVal = (int)Math.Round(fValue, 0);
            return retVal;
        }

        void gameOver(int winner)
        {
            gameIsOver = true;
            if (winner == 2)
            {
                output = "Enough! Both Players Are Down!!!";
            }
            else
            {
                output = "Enough! Player " + (winner + 1) + " Wins!!!";
            }
            output += "\nPress Enter to play again...";
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                player[0].resetStats();
                player[1].resetStats();
                p1position = w1position = 0;
                p2position = w2position = 5;
                p1location = new Vector3(p1position, 0, 0);
                p2location = new Vector3(p2position, 0, 0);
                w1location = new Vector3(w1position, 0, 0);
                w2location = new Vector3(w2position, 0, 0);
                punch1 = punch2 = punch1Animation = punch2Animation = punch1out = punch2out = gameIsOver = false;
                allocatingStatsStage = true;
            }
        }

        void DrawModel(Model model, Texture2D texture, Vector3 scale, Vector3 rotation, Vector3 location)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(location);
                    effect.View = Matrix.CreateLookAt(cameraPosition, avatarPosition, new Vector3(0.0f, 0.0f, 1.0f));
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(0.1f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f);
                    effect.EnableDefaultLighting();
                    //effect.DiffuseColor = pipeColor[i];
                    effect.Texture = texture;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            rotationMatrix = Matrix.CreateRotationZ(avatarYaw);
            transformedReference = Vector3.Transform(thirdPersonReference, rotationMatrix);
            cameraPosition = transformedReference + avatarPosition;
            // TODO: Add your drawing code here
            spriteBatch.Begin();//Background

            Vector2 zeroSpot = new Vector2(0, 0);
            if (allocatingStatsStage)
            {
                spriteBatch.Draw(allocBackground, zeroSpot, Color.White);
                spriteBatch.Draw(allocFloorTexture, floorPos, Color.White);
            }
            else
            {
                spriteBatch.Draw(combatBackground, zeroSpot, Color.White);
                spriteBatch.Draw(combatFloorTexture, floorPos, Color.White);
            }
            spriteBatch.End();

            DrawModel(weapon1model, w1color, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), w1location);
            DrawModel(weapon2model, w2color, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), w2location);
            DrawModel(player1model, p1color, new Vector3(1, 1, 1), new Vector3(0, 0, 0), p1location);
            DrawModel(player2model, p2color, new Vector3(1, 1, 1), new Vector3(0, 0, 0), p2location);

            spriteBatch.Begin();//Foreground


            // Find the center of the string
            FontOrigin = font.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(font, output, fontPos, Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            //Placing a tag underneath the models
            //spriteBatch.DrawString(font, "Player 1", new Vector2(p1location.X + 760f, p1location.Y + 400f), Color.Black, 0, font.MeasureString("Player 1") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
