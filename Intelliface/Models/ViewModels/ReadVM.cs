namespace Intelliface.Models.ViewModels
{
    public class ReadVM<T> where T : class
    {
       public int id { get; set; }
       public T data { get; set; }
    }
}
