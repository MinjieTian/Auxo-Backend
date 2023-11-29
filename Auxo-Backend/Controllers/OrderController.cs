using Auxo_Backend.Dto;
using Microsoft.AspNetCore.Mvc;
using Auxo_Backend.Data;

namespace Auxo_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{

    private readonly IRepo _repo;

    //keep thread safe
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public OrdersController(IRepo repo)
    {
        _repo = repo;
    }
    [HttpPost(Name = "PlaceOrder")]
    public async Task<ActionResult> PlaceOrder(OrderDto order)
    {
        await _semaphore.WaitAsync();
        OrderResult orderResult;
        try
        {
            orderResult = await _repo.PlaceOrder(order);
        }
        catch (Exception err)
        {
            return BadRequest(err);
        }
        finally
        {
            _semaphore.Release();
        }
        return Ok(orderResult);
    }
}