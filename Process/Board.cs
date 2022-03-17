using System;

namespace App_2048 {
    public partial class Board {
        private Case[,] board;

        public Case[,] BoardArray => this.board;
        public int width { get; } = 4 ; // Default width to 4

        // enum to specify current gamestate
        public enum GameState {
            DEFAULT, 
            LOOSE, 
            WIN,
            BLOCK
        }

        public Board() {
            this.board = new Case[this.width,this.width];
            
            Random random1 = new Random();

            // get random first case
            (int firstCaseX, int firstCaseY) = (random1.Next(this.width), new Random().Next(this.width));
            (int secondCaseX, int secondCaseY) = (random1.Next(this.width), new Random().Next(this.width));
            
            for (int i = 0 ; i < board.GetLength(0) ; i++) {
                for (int j = 0 ; j < board.GetLength(1);j ++) {
                    if ((j == firstCaseX && i == firstCaseY) || (j == secondCaseX && i == secondCaseY))
                        this.board[i,j] = new Case(j,i,2);
                    else
                        this.board[i,j] = new Case(j,i,0);
                }
            }

            Case[,] copyBoard = new Case[this.width, this.width];
            for (int i = board.GetLength(0) - 1; i >= 0 ; i--) {
                for (int j = 0; j < board.GetLength(1) ;j++) 
                    copyBoard[i,j] = this.board[board.GetLength(0) - 1 - i,j];
            }

            this.board = copyBoard;
        }

        public GameState Move(Direction direction)
        {
            Board boardBeforeUpdate = this.DeepCopy();
            for (int a = 0 ; a < this.width - 1 ; a ++){
                for (int i = 0 ; i < board.GetLength(0) ; i++) {
                    for (int j = 0 ; j < board.GetLength(1) ;j++) {
                        if (this.board[i,j].value != 0)
                        {
                            int prevX = this.board[i,j].X;
                            int prevY = this.board[i,j].Y;
                            int newX = prevX + DeltaX(direction);
                            int newY = prevY + DeltaY(direction); 

                            if (newX >= 0 && newX < this.width && newY >= 0 && newY < this.width)
                            {
                                if (!this.board[i, j].HasMooved)
                                {
                                    bool ins = this.SetCase(newX, newY, this.board[i,j].value);
                                    if (ins)
                                        this.DelCase(prevX, prevY);
                                }
                            }
                        }
                    }
                }
            }
            
            ResetHasMooved(); 
            GameState currentGameState = GetGameState();
            
            bool equal = BoardAreEquals(boardBeforeUpdate);
            if (!equal && currentGameState == GameState.DEFAULT){
                // insert new case
                InsertNewCase();
            }
            return currentGameState;
        }

        // set new value on case
        private bool SetCase(int newX, int newY, int prevValue) {
            for (int i = 0 ; i < this.board.GetLength(0); i++) {
                for (int j = 0 ; j < this.board.GetLength(1); j++) {
                    if (this.board[i,j].X == newX && this.board[i,j].Y == newY)
                        if (prevValue == this.board[i,j].value && !this.board[i,j].HasMooved){
                            this.board[i,j].value += prevValue;
                            this.board[i,j].HasMooved = true;
                            return true;
                        }
                        else {
                            if (this.board[i,j].value == 0){
                                this.board[i,j].value = prevValue;
                                return true;
                            }
                            return false;
                        }
                }
            }

            return false;
        }

        private void DelCase(int x, int y) {
            for (int i = 0 ; i < this.board.GetLength(0); i++) {
                for (int j = 0 ; j < this.board.GetLength(1); j++) {
                    if (this.board[i,j].X == x && this.board[i,j].Y == y)
                        this.board[i,j].value = 0;
                }
            }
        }

        private Board DeepCopy()
        {
            Board copyBoard = new Board();
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.width; j++)
                    copyBoard.board[i, j].value = this.board[i, j].value;
            }

            return copyBoard;
        }

        private bool BoardAreEquals(Board newBoard)
        {
            bool flag = true;
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (this.board[i, j].value != newBoard.board[i, j].value)
                        flag = false;
                }
            }

            return flag;
        }
        private int DeltaX(Direction direction) {
            switch (direction) {
                case Direction.Right :
                    return 1; 
                case Direction.Left:
                    return -1;
                default:
                    return 0;
            }
        }
        private int DeltaY(Direction direction) {
            switch (direction) {
                case Direction.Down :
                    return -1; 
                case Direction.Up:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}