using Auxo_Backend.Models;
using Auxo_Backend.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Auxo_Backend.Data;

public interface IRepo {

    public Task<IEnumerable<Parts>> GetParts();
    public Task<IEnumerable<Parts>> AddPart(PartsDto partsdto);
    public Task<OrderResult> PlaceOrder(OrderDto orderDto);
}