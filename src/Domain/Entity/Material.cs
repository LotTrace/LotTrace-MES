namespace LotTrace_MES.src.Domain.Entity
{
    public class Material
    {
        public int MaterialId { get; set; }
        public required string Name { get; set; }
        public int Stock { get; set; }
    }
}
