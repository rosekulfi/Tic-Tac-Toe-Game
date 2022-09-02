using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public class GameEngine
    {
        private const float lineLength = 80;
        private const float block = lineLength / 3;
        private const float delta = 5;

        public enum CellSelection { N, O, X };
        public CellSelection[,] grid = new CellSelection[3, 3];

        public bool gameOver;       //game over
        public bool gameTied;       //game tied
        public bool compWin;        //computer wins
        public bool userWin;        //user wins
        public bool compTurn;       //computer's turn
        public bool firstCompTurn;  //computer goes first
        public int moves;           //number of turns

        //arrays for computer and user representing ways to win
        //0: left column          1: middle column        2: right column
        //3: top row              4: middle row           5: bottom row
        //6: diagonal (bottom right to top left)
        //7: diagonal (bottom left to top right)

        //
        //      0   1   2
        //  0   X   X   X
        //  1   X   X   X
        //  2   X   X   X
        //

        public int[] compDubs = new int[8];
        public int[] userDubs = new int[8];

        public GameEngine() 
        {
            gameOver = false;
            gameTied = false;
            compWin = false;
            userWin = false;
            compTurn = false;
            firstCompTurn = false;
            moves = 0;

            //set all array values to 0
            for (int i = 0; i < 8; i++) 
            {
                compDubs[i] = 0;
                userDubs[i] = 0;
            }

        }

        public void UserMove(MouseEventArgs e, PointF[] p, GameEngine tttboard) 
        {
            
            if (p[0].X < 0 || p[0].Y < 0) 
                return;

            int i = (int)(p[0].X / block);
            int j = (int)(p[0].Y / block);

            if (i > 2 || j > 2) 
                return;

            if (gameOver) 
                return;

            else if (tttboard.grid[i, j] == CellSelection.N && !compTurn && !gameOver) //only allow setting empty cells
            {
                if (e.Button == MouseButtons.Left) 
                { 
                    tttboard.grid[i, j] = CellSelection.X;
                    
                    //i = 0
                    if(i == 0) 
                    {
                        userDubs[0]++;

                        //i = 0 & j = 0
                        if (j == 0)
                        {
                            userDubs[6]++;
                        }

                        //i = 0 & j = 2
                        if (j == 2) 
                        {
                            userDubs[7]++;
                        }
                        
                    }

                    //i = 1
                    if(i == 1) 
                    {
                        userDubs[1]++;

                        //i = 1 & j = 1
                        if(j == 1) 
                        {
                            userDubs[6]++;
                            userDubs[7]++;
                        }
                        
                    }

                    //i = 2
                    if(i == 2) 
                    {
                        userDubs[2]++;

                        //i = 2 & j = 0
                        if(j == 0) 
                        {
                            userDubs[7]++;
                        }

                        //i = 2 & j = 2
                        if(j == 0) 
                        {
                            userDubs[6]++;
                        }
                        
                    }

                    //j = 0
                    if(j == 0) 
                    {
                        userDubs[3]++;
                    }

                    //j = 1
                    if(j == 1) 
                    {
                        userDubs[4]++;
                    }

                    //j = 2
                    if(j == 2) 
                    {
                        userDubs[5]++;
                    }

                    compTurn = true;
                    tttboard.moves++;
                    WhoWins(tttboard);
                    CompMove(tttboard);
                }

            }

            else 
            {
                MessageBox.Show("Invalid Move");
            }
        }

        public void CompMove(GameEngine tttboard)
        {
            //MessageBox.Show("Beginning of cpu function");
            if (gameOver) {
                compTurn = false;
                return;
            }
            if(tttboard.moves == 9) 
            {
                WhoWins(tttboard);
                return;
            }

            else if(firstCompTurn && compTurn)
            {
                //MessageBox.Show("Checkpoint 1");
                tttboard.grid[1, 1] = CellSelection.O;
                compDubs[1]++;
                compDubs[4]++;
                compDubs[6]++;
                compDubs[7]++;
                firstCompTurn = false;
                compTurn = false;
                tttboard.moves++;
                return;
            }

            else if(!firstCompTurn && compTurn && !gameOver)
            {
                //--------------CHECK FOR POTENTIAL DUBS!-------------//
                //left column
                //MessageBox.Show("Checkpoint 2");
                if (compDubs[0] == 2 || userDubs[0] == 2)
                {
                    if (tttboard.grid[0, 2] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[0, 1])
                    {
                        //MessageBox.Show("Checkpoint 2.1");
                        tttboard.grid[0, 2] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[5]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 1] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[0, 2])
                    {
                        //MessageBox.Show("Checkpoint 2.2");
                        tttboard.grid[0, 1] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[4]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 0] == CellSelection.N && tttboard.grid[0, 1] == tttboard.grid[0, 2])
                    {
                        //MessageBox.Show("Checkpoint 2.3");
                        tttboard.grid[0, 0] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[3]++;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    //MessageBox.Show("Checkpoint 2 end");
                }

                //middle column
                if (compDubs[1] == 2 || userDubs[1] == 2)
                {
                    //MessageBox.Show("Checkpoint 3");
                    if (tttboard.grid[1, 2] == CellSelection.N && tttboard.grid[1, 0] == tttboard.grid[1, 1])
                    {
                        //MessageBox.Show("Checkpoint 3.1");
                        tttboard.grid[1, 2] = CellSelection.O;
                        compDubs[1]++;
                        compDubs[5]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 1] == CellSelection.N && tttboard.grid[1, 0] == tttboard.grid[1, 2])
                    {
                        //MessageBox.Show("Checkpoint 3.2");
                        tttboard.grid[1, 1] = CellSelection.O;
                        compDubs[1]++;
                        compDubs[4]++;
                        compDubs[6]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 0] == CellSelection.N && tttboard.grid[1, 1] == tttboard.grid[1, 2])
                    {
                        //MessageBox.Show("Checkpoint 3.3");
                        tttboard.grid[1, 0] = CellSelection.O;
                        compDubs[1]++;
                        compDubs[3]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    //MessageBox.Show("Checkpoint 3 end");
                }

                //right column
                if (compDubs[2] == 2 || userDubs[2] == 2)
                {
                    //MessageBox.Show("Checkpoint 4");
                    if (tttboard.grid[2, 2] == CellSelection.N && tttboard.grid[2, 0] == tttboard.grid[2, 1])
                    {
                        tttboard.grid[2, 2] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[5]++;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 1] == CellSelection.N && tttboard.grid[2, 0] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[2, 1] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[4]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 0] == CellSelection.N && tttboard.grid[2, 1] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[2, 0] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[3]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //top row
                if (compDubs[3] == 2 || userDubs[3] == 2)
                {
                    //MessageBox.Show("Checkpoint 5");
                    if (tttboard.grid[2, 0] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[1, 0])
                    {
                        tttboard.grid[2, 0] = CellSelection.O;
                        compDubs[3]++;
                        compDubs[2]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 0] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[2, 0])
                    {
                        tttboard.grid[1, 0] = CellSelection.O;
                        compDubs[3]++;
                        compDubs[1]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 0] == CellSelection.N && tttboard.grid[1, 0] == tttboard.grid[2, 0])
                    {
                        tttboard.grid[0, 0] = CellSelection.O;
                        compDubs[3]++;
                        compDubs[0]++;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //middle row
                if (compDubs[4] == 2 || userDubs[4] == 2)
                {
                    //MessageBox.Show("Checkpoint 6");
                    if (tttboard.grid[2, 1] == CellSelection.N && tttboard.grid[0, 1] == tttboard.grid[1, 1])
                    {
                        tttboard.grid[2, 1] = CellSelection.O;
                        compDubs[4]++;
                        compDubs[2]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 1] == CellSelection.N && tttboard.grid[0, 1] == tttboard.grid[2, 1])
                    {
                        tttboard.grid[1, 1] = CellSelection.O;
                        compDubs[4]++;
                        compDubs[1]++;
                        compDubs[6]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 1] == CellSelection.N && tttboard.grid[1, 1] == tttboard.grid[2, 1])
                    {
                        tttboard.grid[0, 1] = CellSelection.O;
                        compDubs[4]++;
                        compDubs[0]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //bottom row
                if (compDubs[5] == 2 || userDubs[5] == 2)
                {
                    //MessageBox.Show("Checkpoint 7");
                    if (tttboard.grid[2, 2] == CellSelection.N && tttboard.grid[0, 2] == tttboard.grid[1, 2])
                    {
                        tttboard.grid[2, 2] = CellSelection.O;
                        compDubs[5]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 2] == CellSelection.N && tttboard.grid[0, 2] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[1, 2] = CellSelection.O;
                        compDubs[5]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 2] == CellSelection.N && tttboard.grid[1, 2] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[0, 2] = CellSelection.O;
                        compDubs[5]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //diagonal - bottom right to top left
                if (compDubs[6] == 2 || userDubs[6] == 2)
                {
                    //MessageBox.Show("Checkpoint 8");
                    if (tttboard.grid[0, 0] == CellSelection.N && tttboard.grid[1, 1] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[0, 0] = CellSelection.O;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 1] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[2, 2])
                    {
                        tttboard.grid[1, 1] = CellSelection.O;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 2] == CellSelection.N && tttboard.grid[0, 0] == tttboard.grid[1, 1])
                    {
                        tttboard.grid[2, 2] = CellSelection.O;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //diagonal - bottom left to top right
                if (compDubs[7] == 2 || userDubs[7] == 2)
                {
                    //MessageBox.Show("Checkpoint 9");
                    if (tttboard.grid[0, 2] == CellSelection.N && tttboard.grid[1, 1] == tttboard.grid[2, 0])
                    {
                        tttboard.grid[0, 2] = CellSelection.O;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 1] == CellSelection.N && tttboard.grid[0, 2] == tttboard.grid[2, 0])
                    {
                        tttboard.grid[1, 1] = CellSelection.O;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 0] == CellSelection.N && tttboard.grid[0, 2] == tttboard.grid[1, 1])
                    {
                        tttboard.grid[2, 0] = CellSelection.O;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                }

                //---------------NO POSSIBLE DUBS ATM----------------//
                 
                
                    //MessageBox.Show("Checkpoint 10");
                    if (tttboard.grid[0, 0] == CellSelection.N)
                    {
                        tttboard.grid[0, 0] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[3]++;
                        compDubs[6]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 1] == CellSelection.N)
                    {
                        tttboard.grid[0, 1] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[4]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[0, 2] == CellSelection.N)
                    {
                        tttboard.grid[0, 2] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[5]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 0] == CellSelection.N)
                    {
                        tttboard.grid[1, 0] = CellSelection.O;
                        compDubs[0]++;
                        compDubs[4]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 1] == CellSelection.N)
                    {
                        tttboard.grid[1, 1] = CellSelection.O;
                        compDubs[1]++;
                        compDubs[4]++;
                        compDubs[6]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[1, 2] == CellSelection.N)
                    {
                        tttboard.grid[1, 2] = CellSelection.O;
                        compDubs[1]++;
                        compDubs[5]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 0] == CellSelection.N)
                    {
                        tttboard.grid[2, 0] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[3]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 1] == CellSelection.N)
                    {
                        tttboard.grid[2, 1] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[4]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (tttboard.grid[2, 2] == CellSelection.N)
                    {
                        tttboard.grid[2, 2] = CellSelection.O;
                        compDubs[2]++;
                        compDubs[5]++;
                        compDubs[7]++;
                        tttboard.moves++;
                        compTurn = false;
                        WhoWins(tttboard);
                        return;
                    }
                    else if (gameOver) return;
                }
            
        }

        public void WhoWins(GameEngine tttboard) 
        {
            if (!gameOver)
            {
                for(int a = 0; a < 8; a++) 
                {
                    if(userDubs[a] == 3) 
                    {
                        userWin = true;
                    }
                    if(compDubs[a] == 3) 
                    {
                        compWin = true;
                    }
                }
            }

            if (userWin && !compWin)
            {
                MessageBox.Show("Congratulations, You Win!");
                gameOver = true;
            }

            else if (!userWin && compWin)
            {
                MessageBox.Show("You Lose!");
                gameOver = true;
            }
            else if (userWin && compWin)
            {
                MessageBox.Show("You Tied!");
                gameOver = true;
                gameTied = true;
            }
            else if (!userWin && !compWin && tttboard.moves == 9)
            {
                MessageBox.Show("You Tied!");
                gameOver = true;
                gameTied = true;
            }
            else
                return;
        }
    }
}