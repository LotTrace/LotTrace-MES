namespace LotTrace_MES.src.Application.DTO.Response.Material
{
    public class ResponseMaterialDTO
    {
        public int MaterialId { get; set; }
        public required string Name { get; set; } 
        public int Stock { get; set; }
        public bool IsLowStock { get; set; } 
    }
}
