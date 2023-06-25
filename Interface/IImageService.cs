using gym.management.system.api.Models;

namespace gym.management.system.api.Interface
{
    public interface IImageService
    {
        public Task<bool> UploadImage(IFormFile image, string memberid);
        public Task<Image> GetImageAsync(string memberid);
        public Task<bool> UpdateMemberImageAsync(Image image);
    }
}
