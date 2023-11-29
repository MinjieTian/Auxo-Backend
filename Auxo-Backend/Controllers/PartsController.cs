using Auxo_Backend.Models;
using Auxo_Backend.Dto;
using Microsoft.AspNetCore.Mvc;
using Auxo_Backend.Data;

namespace Auxo_Backend.Controllers;

[ApiController]
[Route("[controller]")]

public class PartsController : ControllerBase
{

    private readonly IRepo _repo;

    //keep thread safe
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public PartsController(IRepo repo)
    {
        _repo = repo;
    }

    [HttpGet(Name = "GetParts")]
    public async Task<ActionResult<IEnumerable<Parts>>> GetParts()
    {
        await _semaphore.WaitAsync();
        Task<IEnumerable<Parts>> PartsResult;
        try
        {
            PartsResult = _repo.GetParts();
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
        finally
        {
            _semaphore.Release();
        }
        return Ok(PartsResult);
    }

    [HttpPost(Name = "CreateParts")]

    public async Task<ActionResult<IEnumerable<Parts>>> CreateNewParts(PartsDto partsDto)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _repo.AddPart(partsDto);
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
        finally
        {
            _semaphore.Release();
        }

        return CreatedAtAction(nameof(GetParts), new { }, _repo.GetParts());
    }


}