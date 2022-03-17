using System;

namespace App_2048 {
    public partial class Board {
        // Check if game is over (WIN)
        public GameState GetGameState() {
            int count = 0;
            bool canMoove = false;
            for (int i = 0 ; i < this.width; i ++) {
                for (int j = 0 ; j < this.width; j++) {
                    // check if 2048 case exist
                    if (this.board[i,j].value == 2048)
                        return GameState.WIN;
                    
                    // increment count if case isn't empty
                    if (this.board[i,j].value != 0) count++;

                    // check if user can moove one more time
                    int currentValue = this.board[i,j].value;
                    int up = i - 1, down = i + 1, left = j - 1 , right = j + 1;
                    if (up >= 0)
                        if (this.board[up, j].value == currentValue) canMoove = true; 
                    else if (down < this.width)
                        if (this.board[down, j].value == currentValue) canMoove = true;
                    else if (left >= 0)
                        if (this.board[i, left].value == currentValue) canMoove = true;
                    else if (right < this.width)
                        if (this.board[i, right].value == currentValue) canMoove = true;
                }
            }

            if (count == (this.width * this.width) && canMoove) return GameState.BLOCK;

            return (count == (this.width * this.width)) && !canMoove ? GameState.LOOSE : GameState.DEFAULT; 
        }

        // Insert new case in the board
        private void InsertNewCase() {
            int[] possibleValue = { 2, 2, 2, 4 }; // 1 change of 4 to get 4.  
            int value = possibleValue[new Random().Next(possibleValue.Length)];

            int randomX = new Random().Next(this.width), randomY = new Random().Next(this.width);

            bool InsertAtPosition(int x, int y, int value) {
                for (int i = 0 ; i < this.width; i++) {
                    for (int j = 0 ; j < this.width ; j++)  {
                        if (this.board[i,j].X == x && this.board[i,j].Y == y) {
                            if (this.board[i,j].value == 0) {
                                this.board[i,j].value = value;
                                return true;
                            }else return false;
                        }
                    }
                }

                return false;
            }

            while (!InsertAtPosition(randomX, randomY, value)) {
                randomX = new Random().Next(this.width);
                randomY = new Random().Next(this.width);
            }
        }

        // reset hasMooved
        private void ResetHasMooved() {
            for (int i = 0 ; i < this.width ; i++) {
                for (int j = 0 ; j < this.width ; j++)
                    this.board[i,j].HasMooved = false;
            }
        }
    }
}