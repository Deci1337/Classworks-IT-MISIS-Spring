using System;

namespace GameOFLife.Core
{
    public class Game
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Generation { get; private set; }
        public int Speed { get; set; }
        public bool[,] Field { get; private set; }

        public Game(int rows, int columns, int speed = 1)
        {
            Rows = rows;
            Columns = columns;
            Speed = speed;
            Generation = 0;
            Field = new bool[rows, columns];
        }

        public void SetCell(int row, int col, bool alive)
        {
            Field[row, col] = alive;
        }

        public void NextGeneration()
        {
            bool[,] next = new bool[Rows, Columns];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    int neighbours = CountNeighbours(r, c);

                    if (Field[r, c])
                        next[r, c] = neighbours == 2 || neighbours == 3;
                    else
                        next[r, c] = neighbours == 3;
                }
            }

            Field = next;
            Generation++;
        }

        private int CountNeighbours(int row, int col)
        {
            int count = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;

                    int r = row + dr;
                    int c = col + dc;

                    if (r >= 0 && r < Rows && c >= 0 && c < Columns && Field[r, c])
                        count++;
                }
            }

            return count;
        }
    }
}
