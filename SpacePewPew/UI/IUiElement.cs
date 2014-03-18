using System.Drawing;

namespace SpacePewPew.UI
{
    public interface IUiElement
    {
        void Draw();

        Point Position { get; set; }
    }
}
