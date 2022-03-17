namespace App_2048
{
    public class Case
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int value { get; set; }

        public bool HasMooved { get; set; }

        public Case(int x, int y, int value)
        {
            this.X = x ;
            this.Y = y;
            this.value = value;
            this.HasMooved = false;
        }
        
        public Case Step(Direction direction)
        {
            Case caseCopie = new Case(this.X, this.Y, this.value);

            switch (direction) {
                case Direction.Up :
                    caseCopie.X -= 1; 
                    break;
                case Direction.Down : 
                    caseCopie.X += 1; 
                    break;
                case Direction.Right : 
                    caseCopie.Y += 1; 
                    break;
                case Direction.Left : 
                    caseCopie.Y -= 1;
                    break;
            }

            if (caseCopie.X < 4 && caseCopie.X >= 0 && caseCopie.Y >= 0 && caseCopie.Y < 4)
                return caseCopie; 
            return this;
        }
    }
}
