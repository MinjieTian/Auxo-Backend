using Auxo_Backend.Models;
using Auxo_Backend.Dto;

namespace Auxo_Backend.Data;

public class Repo : IRepo
{

    private readonly static List<Parts> Parts = new List<Parts>
    {
        new Parts { Id = 1, Description = "Wire", Price = 5.99, Quantity = 5 },
        new Parts { Id = 2, Description = "Brake Fluid", Price = 4.90, Quantity = 20 },
        new Parts { Id = 3, Description = "Engine Oil", Price = 15.00, Quantity = 12 }
    };

    public async Task<IEnumerable<Parts>> GetParts()
    {
        return Parts;
    }

    public async Task<IEnumerable<Parts>> AddPart(PartsDto dto)
    {
        var part = Parts.FirstOrDefault(e => e.Description == dto.Description && e.Price == dto.Price );
        if ( part == null) {
            Parts.Add(new Parts { Id = Parts.Count + 1, Description = dto.Description, Price = dto.Price, Quantity = dto.Quantity });
        } else { part.Quantity += dto.Quantity; }
        return Parts;
    }

    // I am trying to find a better way for this function, I want to check all selected parts are with valid number first. It is not a clear way, 
    // cause there are some duplicate codes here. I will improve it.
    public async Task<OrderResult> PlaceOrder(OrderDto orderDto)
    {
        OrderResult resultObj = new() { TotalPrice = 0.0, Items = new List<ItemInOrder>() };
        foreach (var item in orderDto.SelectedParts)
        {
            var part = Parts.FirstOrDefault(p => p.Id == item.Id);
            if (!IsValidPart(item, part, out string errorMessage)) { throw new Exception(errorMessage); }
        }
        foreach (var item in orderDto.SelectedParts)
        {
            var part = Parts.FirstOrDefault(p => p.Id == item.Id);
            part.Quantity -= item.Quantity;
            double priceForThisPart = item.Quantity * part.Price;
            resultObj.TotalPrice += priceForThisPart;
            resultObj.Items.Add(new ItemInOrder { Quantity = item.Quantity, Description = part.Description, Price = priceForThisPart });
        }

        // return resultObj;
        return resultObj;
    }

    private static bool IsValidPart(SelectedPart item, Parts? part, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (part == null || part.Quantity < item.Quantity)
        {
            errorMessage = $"Inventory shortage or Item not exist for Item ID {item.Id}";
            return false;
        }
        return true;
    }

}

public class OrderResult
{
    public double TotalPrice { get; set; }
    public List<ItemInOrder>? Items { get; set; }
}

public class ItemInOrder
{
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}