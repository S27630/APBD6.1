using System.ComponentModel.DataAnnotations;
using WebApplication5.Model;
namespace WebApplication5.DTOs;

public record WarehouseProductDTO(int IdProductWarehouse, int IdWarehouse, int IdProduct, int IdOrder, int Amount, decimal Price, DateTime CreatedAt)
{
    public WarehouseProductDTO(ProductWarehouse productWarehouse) : this(productWarehouse.IdProductWarehouse, productWarehouse.IdWarehouse,
        productWarehouse.IdProduct, productWarehouse.IdOrder, productWarehouse.Amount, productWarehouse.Price, productWarehouse.CreatedAt)
    { }
}
public record CreateProductWarehouseRequest(
    [Required] int IdWarehouse,
    [Required] int IdProduct,
    [Required] int Amount,
    [Required] DateTime CreatedAt
);

public record CreateProductWarehouseResponse(int IdProductWarehouse, int IdWarehouse, int IdProduct, int IdOrder, int Amount, double Price, DateTime CreatedAt)
{
    public CreateProductWarehouseResponse(int IdProductWarehouse, int IdOrder, double Price, CreateProductWarehouseRequest request):this(IdProductWarehouse, 
        request.IdWarehouse, request.IdProduct, IdOrder, request.Amount, Price, request.CreatedAt) {}
};
