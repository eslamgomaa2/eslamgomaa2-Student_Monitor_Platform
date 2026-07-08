using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.FileStorge
{
    public interface IFileStorge
    {
        public Task<string> SaveFileAsync(IFormFile file, string folder);


        public Task DeleteFileAsync(string relativePath);


        public string GetPublicUrl(string relativePath);
    }
}
