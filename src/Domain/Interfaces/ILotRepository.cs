using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface ILotRepository : IRepository<Lot>
    {
        Task<Lot?> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Lot>> GetByStateAsync(LotState state);
        Task<IEnumerable<Lot>> GetByLineIdAsync(int lineId);
        Task<Lot?> GetByBarcodeForUpdateAsync(string barcode);
    }
}