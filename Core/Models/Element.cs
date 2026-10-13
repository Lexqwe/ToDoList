using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models;

public class Element
{
    public string? Title { get; set; }
    public string? Text { get ; set; }
    public List<Element> ElementCategories = new List<Element>();
}
