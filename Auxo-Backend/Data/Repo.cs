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
        Parts.Add(new Parts { Id = Parts.Count + 1, Description = dto.Description, Price = dto.Price, Quantity = dto.Quantity });
        return Parts;
    }

    public async Task<OrderResult> PlaceOrder(OrderDto orderDto)
    {
        OrderResult resultObj = new();
        foreach (var item in orderDto.SelectedParts)
        {
            var part = Parts.FirstOrDefault(p => p.Id == item.Id);
            if (!IsValidPart(item, part, out string errorMessage))
            {
                resultObj.errMessage = errorMessage;
                return resultObj;
            }
            else
            {
                part.Quantity -= item.Quantity;
                if (resultObj.SelectedPartsRes == null) { resultObj.SelectedPartsRes = new List<SelectedPart> { new() { Id = item.Id, Quantity = item.Quantity } }; }
                else
                {
                    resultObj.SelectedPartsRes.Add(new SelectedPart { Id = item.Id, Quantity = item.Quantity });
                }
            }
        }

        return resultObj;
    }

    private bool IsValidPart(SelectedPart item, Parts? part, out string errorMessage)
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
    public List<SelectedPart>? SelectedPartsRes { get; set; }
    public string? errMessage { get; set; }
}