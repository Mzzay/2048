using System;
using System.Collections.Generic;

namespace App_2048 {
    public partial class Board {
        // print board
        public string PadCenter(string s, int width, char c)
        {
            List<char> content = new List<char>(); 
            foreach (char l in s)
                content.Add(l);
            
            // rest to insert
            int rest = width - content.Count;
            if (rest < 0)
                return string.Join("", content);

            for (int i = 0; i < rest; i++)
            {
                if (i % 2 == 0)
                    content.Insert(0,c);
                else
                    content.Add(c);
            }

            return string.Join("", content);
        }
        
        public void PrintLine(int i, int width, int longest_number)
        {
            string leftCorner = "╔";
            string rightCorner = "╗";
            string middle = "═";
            string separator = "╦";

            if (i == width)
            {
                leftCorner = "╚";
                rightCorner = "╝";
                separator = "╩";
            } else if (i != 0)
            {
                leftCorner = "╠";
                rightCorner = "╣";
                separator = "╬";
            }

            List<string> content = new List<string>();

            for (int j = 0; j < width; j++)
            {
                if (j == 0)
                    content.Add(leftCorner);
                for (int k = 0; k < longest_number ; k++)
                    content.Add(middle);
                if (j == width - 1)
                    content.Add(rightCorner);
                else
                    content.Add(separator);
            }
            
            Console.WriteLine(string.Join("", content));
        }
        
        public void Print()
        {
            string separator = "║";
            int count = 6;
            
            for (int i = 0; i < 4; i++)
            {
                PrintLine(i,this.width, count + 2);
                List<string> lineContent = new List<string>();
                lineContent.Add(separator);
                for (int j = 0; j < this.width; j++)
                {
                    string value = this.board[i,j].value.ToString() == "0"
                        ? ""
                        : this.board[i,j].value.ToString();
                    lineContent.Add(PadCenter(value, count + 2, ' '));
                    lineContent.Add(separator);
                }
                Console.WriteLine(string.Join("", lineContent));
            }

            PrintLine(this.width, this.width, count + 2);
        }
        public void Show() {
            Console.WriteLine("--- Current Board ---");
            for (int i = 0 ; i < board.GetLength(0) ; i++) {
                string line = "" ;                
                for (int j = 0 ; j < board.GetLength(1) ; j++) {
                    line += string.Format("{0} | ", board[i,j].value);
                }
                Console.WriteLine(line);
            }
        }
    }
}