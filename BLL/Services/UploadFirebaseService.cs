using BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Firebase.Storage;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Drawing;
using System.Net.Http;

namespace BLL.Services
{
    public class UploadFirebaseService : IUploadFirebaseService
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string bucket;

        public UploadFirebaseService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            bucket = _configuration.GetValue<string>("Firebase:Bucket");
        }


        /// <summary>
        /// Upload File To Firebase
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadFileToFirebase(string file, string type, string parent, string fileName)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    try
                    {
                        var bytes = Convert.FromBase64String(file);

                        FirebaseStorageTask task = new FirebaseStorage(bucket)
                            .Child(type)
                            .Child(parent)
                            .Child(fileName)
                            .PutAsync(new MemoryStream(bytes));
                        return await task;
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message + "\nCannot upload image to Firebase Storage");
                        return null;
                    }

                }
            }
            return String.Empty;
        }


        /// <summary>
        /// Upload Files To Firebase
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadFilesToFirebase(string[] files, string type, string parent, string fileName)
        {
            string urlConcat = String.Empty;
            foreach (var file in files)
            {
                try
                {
                    string url = await UploadFileToFirebase(file, type, parent,
                        fileName + (Array.IndexOf(files, file) + 1));

                    if (file == files[files.Length - 1])
                    {
                        urlConcat += url;
                    }
                    else
                    {
                        urlConcat += url + "|";
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message + "\nCannot upload images to Firebase Storage");
                    return null;
                }
            }
            return urlConcat;
        }
    }
}
