namespace Auxo_Backend.Models;

 public class Parts {
    public int Id { get; set;}

    public required string Description { get; set;}

    public double Price { get; set; }

    public int Quantity { get; set; }
 }