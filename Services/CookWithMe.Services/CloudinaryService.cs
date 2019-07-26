namespace CookWithMe.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public async Task<string> UploadPhotoAsync(IFormFile file, string fileName, string folder)
        {
            byte[] destinationData;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationData = memoryStream.ToArray();
            }

            UploadResult uploadResult = null;

            using (var memoryStream = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = folder,
                    File = new FileDescription(fileName, memoryStream),
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
