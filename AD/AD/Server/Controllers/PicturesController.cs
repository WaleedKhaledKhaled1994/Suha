using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Image;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AD.Server.Controllers
{
    public class PicturesController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFileStorageService _fileService;

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Picture> _pictureRepository;
        public PicturesController(IRepository<Category> categoryRepository, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IRepository<Product> productRepository, IRepository<Picture> pictureRepository, IFileStorageService fileService, IRepository<Country> countryRepository, IRepository<Store> storeRepository)
        {
            _categoryRepository = categoryRepository;
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
            _fileService = fileService;
            _countryRepository = countryRepository;
            _storeRepository = storeRepository;
        }

        [HttpPost("Category/AddImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> PostImageCategory([FromForm] AddImageVMRequest input)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                ApiResponse result;

                string[] AllowedExtentions = { "jpg", "jpeg", "png" };

                var imageExtention = input.Image.FileName.Split('.').Last().ToLower();

                if (!AllowedExtentions.Contains(imageExtention))
                {
                    string message = "Validation Error, Please Enter a Valid Image!";
                    result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                    return Ok(result);
                }

                var entity = await _categoryRepository.GetByIdAsync(input.Id);
                entity.Image = InsertPicture(input.Image, "categories", entity.Code, "jpg");
                await _categoryRepository.UpdateAsync(entity);
                result = ApiResponse.Create(HttpStatusCode.OK, entity);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpPost("Country/AddImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> PostImageCountry([FromForm] AddImageVMRequest input)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                ApiResponse result;

                string[] AllowedExtentions = { "jpg", "jpeg", "png" };

                var imageExtention = input.Image.FileName.Split('.').Last().ToLower();

                if (!AllowedExtentions.Contains(imageExtention))
                {
                    string message = "Validation Error, Please Enter a Valid Image!";
                    result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                    return Ok(result);
                }

                var entity = await _countryRepository.GetByIdAsync(input.Id);
                entity.Image = InsertPicture(input.Image, "countries", entity.Code, "jpg");
                await _countryRepository.UpdateAsync(entity);
                result = ApiResponse.Create(HttpStatusCode.OK, entity);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpPost("Store/AddImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> PostImageStore([FromForm] AddImageVMRequest input)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                ApiResponse result;

                string[] AllowedExtentions = { "jpg", "jpeg", "png" };

                var imageExtention = input.Image.FileName.Split('.').Last().ToLower();

                if (!AllowedExtentions.Contains(imageExtention))
                {
                    string message = "Validation Error, Please Enter a Valid Image!";
                    result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                    return Ok(result);
                }

                var entity = await _storeRepository.GetByIdAsync(input.Id);
                entity.Image = InsertPicture(input.Image, "stores", entity.Code, "jpg");
                await _storeRepository.UpdateAsync(entity);
                result = ApiResponse.Create(HttpStatusCode.OK, entity);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpPost("Product/AddImages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> PostImagesProduct([FromForm] AddImagesVMRequest input)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                ApiResponse result;

                //check count
                if (input.Images.Count > 6)
                {
                    string message = "Validation Error, Please Enter Less than /6/ Images!";
                    result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                    return Ok(result);
                }

                //check validate
                string[] AllowedExtentions = { "jpg", "jpeg", "png" };
                foreach (var item in input.Images)
                {
                    var imageExtention = item.FileName.Split('.').Last().ToLower();

                    if (!AllowedExtentions.Contains(imageExtention))
                    {
                        string message = "Validation Error, Please Enter a Valid Image!";
                        result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                        return Ok(result);
                    }
                }

                //add images
                var entity = await _productRepository.GetByIdAsync(input.Id);
                var imagesURLs = await InsertPicturesAsync(input.Images, "products/Product1", entity.Code, "jpg");
                entity.Image = entity.Image = JsonConvert.SerializeObject(imagesURLs);
                await _productRepository.UpdateAsync(entity);

                result = ApiResponse.Create(HttpStatusCode.OK, entity);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpPost("Product/EditImages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> PutImagesProduct([FromForm] EditImagesVMRequest input)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                ApiResponse result;
                string path = "Products/Product1";
                //check count
                if (input.Images.Count > 6)
                {
                    string message = "Validation Error, Please Enter Less than /6/ Images!";
                    result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                    return Ok(result);
                }

                //check validate
                string[] AllowedExtentions = { "jpg", "jpeg", "png" };
                foreach (var item in input.Images)
                {
                    var imageExtention = item.FileName.Split('.').Last().ToLower();

                    if (!AllowedExtentions.Contains(imageExtention))
                    {
                        string message = "Validation Error, Please Enter a Valid Image!";
                        result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, message);
                        return Ok(result);
                    }
                }

                var entity = await _productRepository.GetByIdAsync(input.Id);
                List<string> imagesURLs = new();

                //delete images
                var dbImages = JsonConvert.DeserializeObject<List<string>>(entity.Image);
                foreach (var item in dbImages)
                {
                    if (input.OldImages.Contains(item))
                        imagesURLs.Add(item);
                    else
                    {
                        int pFrom = item.IndexOf(path) + path.Length + 1;
                        int pTo = item.LastIndexOf("\\");
                        string containerName = item.Substring(pFrom, pTo - pFrom);
                        await _fileService.DeleteFile(item, $"{path}/{containerName}");
                    }
                }

                //add images
                var newImages = await InsertPicturesAsync(input.Images, path, entity.Code, "jpg");
                foreach (var item in newImages)
                    imagesURLs.Add(item);

                entity.Image = JsonConvert.SerializeObject(imagesURLs);

                await _productRepository.UpdateAsync(entity);

                result = ApiResponse.Create(HttpStatusCode.OK, entity);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        #region Helper
        public string InsertPicture(IFormFile file, string containerName, string name = null, string ext = null)
        {
            if (file == null || !file.ContentType.ToLower().StartsWith("image/"))
                throw new BadImageFormatException("Only Images are Allowed");

            string imageName;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ext))
                imageName = DateTime.Now.Ticks + "_" + file.ContentType.Replace("/", ".", StringComparison.OrdinalIgnoreCase);
            else
                imageName = $"{name}.{ext}";

            return InsertPictureToPath(file, containerName, imageName);
        }
        public async Task<List<string>> InsertPicturesAsync(List<IFormFile> files, string containerName, string name = null, string ext = null)
        {
            List<string> imagesURLs = new();

            foreach (var item in files)
                if (item == null || !item.ContentType.ToLower().StartsWith("image/"))
                    throw new BadImageFormatException("Only Images are Allowed");

            string imageName;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ext))
                imageName = DateTime.Now.Ticks + "_" + files[0].ContentType.Replace("/", ".", StringComparison.OrdinalIgnoreCase);
            else
                imageName = $"{name}.{ext}";

            foreach (var item in files)
            {
                Picture pic = new();
                var newPic = await _pictureRepository.InsertAsync(pic);
                var folder = newPic.Id % 10000;
                var newPath = $"{containerName}/{folder}";
                imagesURLs.Add(InsertPictureToPath(item, newPath, imageName));
            }
            return imagesURLs;
        }
        private string InsertPictureToPath(IFormFile file, string containerName, string imageName)
        {
            string folder = Path.Combine(env.WebRootPath, containerName);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string savingPath = Path.Combine(folder, imageName);

            using var fileStream = new FileStream(savingPath, FileMode.Create);
            file.CopyTo(fileStream);

            var currentUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var pathForDatabase = Path.Combine(currentUrl, containerName, imageName);
            return pathForDatabase;
        }
        #endregion
    }
}
