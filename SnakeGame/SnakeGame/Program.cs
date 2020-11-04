using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace SnakeGame
{
    // Structure position to enable arrays for obstacles and foods
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Boolean status = true;
            while(status){
            //Main menu
            Console.WriteLine("Welcome to Snake Game");
            Console.WriteLine("---------------------");
            Console.WriteLine("1.Start game \n2.View scoreboard \n3.Help \n4.Exit");
            string sel = Console.ReadLine();
            if(sel == "1"){    
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // location info & display
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            Position[] directions = new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };
            int direction = right;
            int x = 0, y = 2; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            int lastFoodTime = 0;
            int foodDissapearTime = 8000;
            lastFoodTime = Environment.TickCount;
			


            // clear to color
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

                //border
			    int border_x = 79;
			    int border_y = 24;
                while(border_x > 0){
                    Console.ForegroundColor = ConsoleColor.Blue;
				    Console.SetCursorPosition(border_x, 1);
				    Console.WriteLine("0");
                    Console.SetCursorPosition(border_x, 24);
				    Console.WriteLine("0");
			    	border_x -= 1;
			    }
			        while(border_y > 0){
			    	Console.SetCursorPosition(0, border_y);
				    Console.WriteLine("0");
                    Console.SetCursorPosition(79, border_y);
				    Console.WriteLine("0");
				    border_y -= 1;
			    }
          
            // Obstacles
            List<Position> obstacles = new List<Position>()
            {
                new Position(12, 12),
                new Position(14, 20),
                new Position(7, 7),
                new Position(19, 19),
                new Position(6, 9),
            };
            foreach (Position obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.Write("||");
            }

            // display this char on the console during the game
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 3; i++)
            {
                snakeElements.Enqueue(new Position(2, i));
            }
            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.Write("*");
            }
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // FOOD Display
			// Draw the food at random position 
			// Changes the position if the food is at the same position with obstacles/snake.
            Random random = new Random();
            Position food;
            do
            {
                food = new Position(random.Next(2, consoleHeightLimit),random.Next(0, consoleWidthLimit));
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("F");

            

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 100;

            while(true) // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = cc;
                // Display the score on the top corner when the game is running
                // Calculate how the scores are being counted
                int score = (snakeElements.Count - 4) * 100;
                if (score < 0) score = 0;
                score = Math.Max(score, 0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(68, 0);
                Console.WriteLine("Score:{0}", score);


                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {
                      
                        case ConsoleKey.UpArrow: //UP
                            if (direction != down) direction = up;
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            if (direction != up) direction = down;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            if (direction != right) direction = left;
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            if (direction != left) direction = right;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case ConsoleKey.Enter:
                            gameLive = false;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // calculate the new position
                // note x set to 0 because we use the whole width, but y set to 1 because we use top row for instructions
                x += dx;
                if (x > consoleWidthLimit)
                    x = 0;
                if (x < 0)
                    x = consoleWidthLimit;

                y += dy;
                if (y > consoleHeightLimit)
                    y = 2; // 2 due to top spaces used for directions
                if (y < 2)
                    y = consoleHeightLimit;

                // write the character in the new position
                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];

                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                    snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0) snakeNewHead.col = consoleWidthLimit - 1;
                if (snakeNewHead.row < 2) snakeNewHead.row = consoleHeightLimit - 1;
                if (snakeNewHead.row >= consoleHeightLimit) snakeNewHead.row = 2;
                if (snakeNewHead.col >= consoleWidthLimit) snakeNewHead.col = 0;

                // End the game if the snake make contact with the obstacle or itself.
				// Only 1 life is given for the player.
                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(36,12);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over!");
                    Console.SetCursorPosition(36,13);
                    Console.WriteLine("Your points are: {0}", score);
                    Console.SetCursorPosition(36,14);
                    Console.WriteLine("Press Enter/Escape to Exit");
                    Console.ReadKey();
                    return;
                }

                // Winning requirement
                // When player gain the score specified, the game is cleared
                if (score == 1000 )
                {
                    Console.SetCursorPosition(36,12);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Mission Cleared!");
                    Console.SetCursorPosition(36,13);
                    Console.WriteLine("Your points are: {0}", score);
                    Console.SetCursorPosition(36,14);
                    Console.WriteLine("Press Enter/Escape to Exit");
                    Console.ReadKey();
                    return;
                }

                Console.SetCursorPosition(snakeHead.col,snakeHead.row);
                Console.Write("*");
                snakeElements.Enqueue(snakeNewHead);

                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    // feeding the snake
                    do
                    {
                        food = new Position(random.Next(2, consoleHeightLimit),random.Next(0, consoleWidthLimit));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("F");

                    Position obstacle = new Position();
                    do
                    {
                        obstacle = new Position(random.Next(2, consoleHeightLimit),random.Next(0, consoleWidthLimit));
                    }
                    while (snakeElements.Contains(obstacle) ||
                        obstacles.Contains(obstacle) ||
                        (food.row != obstacle.row && food.col != obstacle.row));
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.col, obstacle.row);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("||");
                }
                else
                {
                    // find the current position in the console grid & erase the character there if don't want to see the trail
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }
                    // Change location of food after specific time/interval
                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(random.Next(0, consoleHeightLimit),random.Next(0, consoleWidthLimit));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }
                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("F");

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive == true);
            }else if(sel == "2"){
                Console.WriteLine("Snake Game Scoreboard Page");
                Console.WriteLine("--------------------------");
                Console.WriteLine("\nPress any key to exit");
                Console.ReadLine();
            }else if(sel == "3"){
                Console.WriteLine("Snake Game Instruction Page");
                Console.WriteLine("---------------------------");
                Console.WriteLine("1. Use 'up', 'down', 'left', 'right' arrow keys to control the snake.");
                Console.WriteLine("2. Avoid the obstacles or the body of the snake, if the head of the snake hits either object, you lose the game immediately.");
                Console.WriteLine("3. Eat the food 'F' to grow the size of the snake, you will also get 100 points from it.");
                Console.WriteLine("4.The food changes its location randomly every 8 seconds if not eaten, pay attention to the food.");
                Console.WriteLine("5. Get 1000 points and you win the game.");
                Console.WriteLine("6. Have fun and enjoy the game.");
                Console.WriteLine("\nPress any key to exit");
                Console.ReadLine();
            }else if(sel == "4"){
                status = false;
            }
        }
    }
    }
}
    
