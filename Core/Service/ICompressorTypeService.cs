using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompressorTypeService
    {
        void Edit(CompressorType compressorType);

        int Insert(CompressorType compressorType);

        CompressorType Get(int idcompressorType);

        void Remove(int idcompressorType);

        IEnumerable<CompressorType> GetByName(string name);
        IEnumerable<CompressorType> GetAll();

        IEnumerable<CompressorTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
