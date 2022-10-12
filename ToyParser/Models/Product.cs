namespace ToyParser.Models;

public class Product
{
    public string Region { get; set; }
    public string[] Breadcrumbs { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    public double? Price { get; set; }
    public double? OldPrice { get; set; }
    public string[] Images { get; set; }
    public string Url { get; set; }
}