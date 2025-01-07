using gym.management.system.api.Interface;
using gym.management.system.api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace gym.management.system.api.Services
{
    public class ImageService : IImageService
    {
        public readonly IMembersService _membersService;
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Image> _imageCollection;
        public ImageService(IConfiguration configuration, IMembersService membersService)
        {
            _membersService = membersService;
            _configuration = configuration;
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString(KeyStore.Configuration.ConnectionStringField));
            IMongoDatabase mongoDatabase = dbClient.GetDatabase(_configuration.GetConnectionString(KeyStore.Configuration.DatabaseNameField));
            _imageCollection = mongoDatabase.GetCollection<Image>(KeyStore.Configuration.Image);
        }

        public async Task<bool> UploadImage(IFormFile image,string memberid)
        {
            if (image != null && image.Length > 0)
            {
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                // Create an instance of the Image model
                var imageModel = new Image
                {
                    Id = memberid,
                    FileName = image.FileName,
                    Data = imageData
                };

                // Save the image to MongoDB
                await _imageCollection.InsertOneAsync(imageModel);

                return true;
            }

            return false;
        }

        public async Task<Image> GetImageAsync(string memberid)
        {
            var filter = Builders<Image>.Filter.Eq("_id", ObjectId.Parse(memberid));
            var imageModel = await _imageCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return imageModel;
        }


        public async Task<bool> UpdateMemberImageAsync(Image image)
        {
            try
            {
                FilterDefinition<Image> filter = Builders<Image>.Filter.Eq("Id", image.Id);

                //var update = Builders<Image>.Update.Set("FileName", image.FileName)
                //                       .Set("Data", image.Data);
                //UpdateResult result = await _imageCollection.UpdateManyAsync(filter, update);

                await _imageCollection.ReplaceOneAsync(filter, image);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
