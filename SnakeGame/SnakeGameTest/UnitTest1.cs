using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace SnakeGameTest
{
    [TestClass]
    public class UnitTest1
    {
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
        [TestMethod]
        public void SnakeDirectionTest()
        {
                // get key and use it to set options
            ConsoleColor color = ConsoleColor.Gray;
                ConsoleKey consoleKey = ConsoleKey.RightArrow;
                switch (consoleKey)
                {
                    case ConsoleKey.UpArrow: //UP
                        color = ConsoleColor.Red;
                        break;
                    case ConsoleKey.DownArrow: // DOWN
                        color = ConsoleColor.Cyan;
                        break;
                    case ConsoleKey.LeftArrow: //LEFT
                        color = ConsoleColor.Green;
                        break;
                    case ConsoleKey.RightArrow: //RIGHT
                        color = ConsoleColor.Black;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Escape: //END
                        break;
                }
                Assert.AreEqual(ConsoleColor.Black, color);
        }

        [TestMethod]
        public void WinningRequirementTest()
        {
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(2, i));
            }
            int score = (snakeElements.Count - 4) * 100;
            Assert.AreEqual(200, score);
        }

        [TestMethod]
        public void RandomFoodPositionTest()
        {
            Random random = new Random();
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            Position food;
            Position position = new Position(5, 5);
            food = new Position(5, 5);
            Assert.AreEqual(position, food);
            food = new Position(random.Next(2, consoleHeightLimit), random.Next(0, consoleWidthLimit));
            Assert.AreNotEqual(position, food);
        }

        [TestMethod]
        public void RandomObstaclePositionTest()
        {
            Random random = new Random();
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            Position obstacle;
            Position position = new Position(5, 5);
            obstacle = new Position(5, 5);
            Assert.AreEqual(position, obstacle);
            obstacle = new Position(random.Next(2, consoleHeightLimit), random.Next(0, consoleWidthLimit));
            Assert.AreNotEqual(position, obstacle);
        }

        [TestMethod]
        public void LifeCountTest()
        {
            string str = "";
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(2, i));
            }
            Position snakeNewHead = new Position(2, 2);
            List<Position> obstacles = new List<Position>()
            {
                new Position(12, 12),
                new Position(14, 20),
                new Position(7, 7),
                new Position(19, 19),
                new Position(6, 9),
            };
            int lifecount = 3;
            if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
            {
                lifecount -= 1;
            }
            if (lifecount == 2)
            {
                str = "Test";
            }
            Assert.AreEqual("Test", str);
        }
    }
}
