

namespace UI
{
    public partial class MainPage : ContentPage
    {
        List<Element> ElementCategories = new();
       
        public MainPage()
        {

            InitializeComponent();
        }
        
        


    }
    public class Element
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
