using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FurnitureFactory
{
    public static class FileHelper
    {
        private static IWebHostEnvironment _appEnvironment;

        private static bool IsInitialized { get; set; }

        public static void Initialize(IWebHostEnvironment hostEnvironment)
        {
            if (IsInitialized)
                throw new InvalidOperationException("Object already initialized");

            _appEnvironment = hostEnvironment;
            IsInitialized = true;
        }

        public static async Task CreateFile(string[] directory, string fileName, IFormFile uploadedFile)
        {
            var path = GetPathWithFileName(directory, fileName);
            await using var fileStream = new FileStream(path, FileMode.Create);
            try
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            catch (Exception)
            {
                throw new Exception("Error to crate file");
            }
        }

        public static async Task DeleteFile(string[] directory, string fileName)
        {
            var path = GetPathWithFileName(directory, fileName);
            {
                try
                {
                    if (File.Exists(path)) File.Delete(path);
                }
                catch (Exception e)
                {
                    throw new Exception("Error to delete file");
                }
            }
        }

        public static string GetPathWithFileName(string[] directory, string fileName)
        {
            try
            {
                return Path.Combine(_appEnvironment.WebRootPath, string.Join('/', directory), fileName);
            }
            catch (Exception)
            {
                throw new Exception("Path not found");
            }
        }

        public static string ToHash(this string content)
        {
            // Use input string to calculate MD5 hash
            using var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(content);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string GetHashFileName(this IFormFile file) =>
            (file.Name + DateTime.Now).ToHash() + Path.GetExtension(file.FileName);
    }
}