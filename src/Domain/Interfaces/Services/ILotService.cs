using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces.Services
{
    public interface ILotService : IService<Lot>
    {
        Task<Lot?> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Lot>> GetByStateAsync(LotState state);
        Task<IEnumerable<Lot>> GetByLineIdAsync(int lineId);
    }
}
