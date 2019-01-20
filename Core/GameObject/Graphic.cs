namespace Core.GameObject
{
    public class GraphicsComponent
    {
        public char Character;
        public RgbColor ForeColor;
        public RgbColor BackColor;

        public GraphicsComponent(char character, RgbColor foreColor)
        {
            Character = character;
            ForeColor = foreColor;
        }
    }
}
