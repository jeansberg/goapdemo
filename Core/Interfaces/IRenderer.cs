using Core.GameObject;

namespace Core
{
    public interface IRenderer
    {
        void Draw(GraphicsComponent graphic, Point point);
        void DrawLine(int x1, int y1, int x2, int y2);
        void LightUp(int x, int y);
        void Highlight(int x, int y);
        void ResetBackGround();
        void ShowTarget(int x, int y, int index);
    }
}