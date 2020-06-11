using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using BookStoreProject.Commons;
using System.Threading.Tasks;
using SQLitePCL;
using System.IO;

namespace BookStoreProject.Services
{
    public interface IImageFileService
    {
        Task<string> UploadImage(IFormFile file);
        bool DeleteImage(string fileName);
    }
    public class ImageFileService : IImageFileService
    {
        private IImageWriter _imageWriter;
        public ImageFileService(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var result = await _imageWriter.UploadImage(file);
            return result;

        }

        public bool DeleteImage(string fileName)
        {
            return _imageWriter.DeleteImage(fileName);
        }

       
    }
}
