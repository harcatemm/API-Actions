using APP.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Interfaces
{
    public interface IProductQueryService
    {
        Task<ProductWithCategoryDTO?> GetByIdWithCategoryDTOAsync(int id, CancellationToken ct = default);
    }
}
