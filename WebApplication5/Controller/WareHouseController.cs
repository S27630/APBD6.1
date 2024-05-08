using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.DTOs;
using WebApplication5.Service;

namespace WebApplication5.Controller;
[ApiController]
[Route("controller-warehouse")]
public class WareHouseController
{

    private readonly IDbService _dbService;

    public WareHouseController(IDbService service)
    {
        _dbService = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductWarehouse(CreateProductWarehouseRequest request)
    {
        return Ok(await _dbService.InsertSomething(request));
    }
}
