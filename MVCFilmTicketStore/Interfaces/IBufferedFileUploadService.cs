using MVCFilmTicketStore.DataTypes.Enums;

namespace MVCFilmTicketStore.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<string> UploadFile(IFormFile file, IWebHostEnvironment webHostEnvironment, FolderType folder);
    }
}
