
using Core.Models;

namespace Core.Services;

public interface IData
{
    public List<Element> DataList { get; }
    public bool AddToList(Element element);
    public bool RemoveFromList(Element element);
    public bool EditElement();
}
