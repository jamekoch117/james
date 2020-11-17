using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace james
{
    public partial class GameScreen : UserControl
    {
        //player button control keys 
        Boolean leftArrowDown, rightArrowDown;
        Item player;
        int playerX, playerY, playerSize, playerSpeed, playerHealth, playerScore; //player variables
        int meteorSize, meteorSpeed, meteorX; //meteor variables
        int boostSize, boostSpeed, boostDraw, boostX; //boost variables
        List<Item> meteors = new List<Item>(); //list of meteors
        List<Item> boosts = new List<Item>(); //list of health boosts
        SolidBrush playerBrush = new SolidBrush(Color.LightBlue); //player brush
        SolidBrush meteorBrush = new SolidBrush(Color.DarkRed); //meteor brush
        SolidBrush boostBrush = new SolidBrush(Color.Yellow); //boost brush
        Random randGen = new Random(); //random generator for all random values 

        

        SoundPlayer boostPlayer = new SoundPlayer(Properties.Resources.Boost); //sound that plays when player collides with a boost
        SoundPlayer hitPlayer = new SoundPlayer(Properties.Resources.Hit); //sound that plays when player collides with a meteor
        SoundPlayer deathPlayer = new SoundPlayer(Properties.Resources.Death); //sound that plays when player dies
        Image james = Properties.Resources.james_koch_icon;

        public GameScreen()
        {
            InitializeComponent();
            leftArrowDown = rightArrowDown = false;
            InitializeGameValues();
            ScoreboardValues();
            MakeMeteors();
        }

        public void InitializeGameValues()
        {
            //Initial player values 
            playerX = this.Width / 2;
            playerY = this.Height - 61;
            playerSize = 30;
            playerSpeed = 10;
            playerHealth = 3;
            playerScore = 1;
            player = new Item(playerX, playerY, playerSize); //player item

            //Initial meteor values 
            meteorSize = 25;
            meteorSpeed = 2;

            //Initial boost values
            boostSize = 30;
            boostSpeed = 8;
        }

        public void ScoreboardValues()
        {
            //Scoreboard values at the start of each level
            healthLabel.Text = "Health: " + playerHealth;
            scoreLabel.Text = "Score: " + playerScore;
        }

        public void MakeMeteors()
        {
            meteorX = randGen.Next(1, 376); //Gives x value for boost

            Color c = Color.DarkRed;

            Item newMeteor = new Item(meteorX, 0, meteorSize, c);
            meteors.Add(newMeteor);
        }

        public void MakeBoosts()
        {
            boostX = randGen.Next(1, 376); //Gives x value for boost

            Color c = Color.Yellow;

            Item newBoost = new Item(boostX, 0, boostSize, c);
            boosts.Add(newBoost);
        }

        public void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        public void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }



        private void gameTimer_Tick_1(object sender, EventArgs e)
        {
            //move main character 
            if (leftArrowDown)
            {
                player.Move("left"); //move left
            }           
            if (rightArrowDown)
            {
                player.Move("right"); //move right
            }


            if (meteors[meteors.Count - 1].y > 21)
            {
                MakeMeteors();
            }

            foreach (Item i in meteors)
            {
                i.Fall(meteorSpeed);
            }

            //make meteors faster if player score surpases a multiple of 50
            if (playerScore % 50 == 0)
            {
                Boolean speed = true;
                if (speed)
                {
                    meteorSpeed++;
                    speed = false;
                }
            }

            if (meteors[0].y > this.Height - 15)
            {
                meteors.RemoveAt(0);

                //player gets a point because they have not been hit by that meteor
                playerScore = playerScore + 1;
                scoreLabel.Text = "Score: " + playerScore;
            }

            Rectangle playerRec = new Rectangle(player.x, player.y, playerSize, playerSize);

            //checking to see if the player gets hit by a meteor
            foreach (Item i in meteors)
            {
                Rectangle meteorRec = new Rectangle(i.x, i.y, meteorSize, meteorSize);
                if (playerRec.IntersectsWith(meteorRec))
                {
                    hitPlayer.Play();
                    playerHealth = playerHealth - 1;
                    if(playerHealth == 0)
                    {
                        Form f = this.FindForm();
                        f.Controls.Remove(this);

                        GameOverScreen go = new GameOverScreen();
                        f.Controls.Add(go);

                    }
                    healthLabel.Text = "Health: " + playerHealth;
                    meteors.Remove(i);
                    break;
                }
            }

            //see if a boost should be made
            boostDraw = randGen.Next(1, 100);

            //add new health boost to health boost list 
            if (boostDraw == 1)
            {
                MakeBoosts();
            }

            //move health boosts
            foreach (Item i in boosts)
            {
                i.Fall(boostSpeed);
            }

            foreach (Item i in boosts)
            {
                Rectangle boostRec = new Rectangle(i.x, i.y, boostSize, boostSize);
                if (playerRec.IntersectsWith(boostRec))
                {
                    boostPlayer.Play();
                    playerHealth++;
                    healthLabel.Text = "Health: " + playerHealth;
                    boosts.Remove(i);
                    break;
                }
            }



            Refresh();
        }
       

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw player to screen
            e.Graphics.FillRectangle(playerBrush, player.x, player.y, player.size, player.size);

            //draw meteor to screen
            foreach (Item m in meteors)
            {
                e.Graphics.DrawImage(james, m.x, m.y, m.size, m.size);
            }

            //draw boost to screen
            foreach (Item b in boosts)
            {
                e.Graphics.FillEllipse(boostBrush, b.x, b.y, b.size, b.size);
            }
        }

    }
}

