using System.Collections.Generic;

namespace SpacePewPew.UI.Proxy.UICreators
{
    public interface IUiCreator
    {
        void Create(List<UiElement> components, List<UiElement> modalComponents);
    }
}
