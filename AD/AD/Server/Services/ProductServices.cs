using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AD.Server.Data;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IRepository<Product> _entityRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly ICategoryServices _categoryServices;
        private readonly IFileStorageService _fileService;
        private string path = "products/Product1";

        public ProductServices(IRepository<Product> entityRepository, IFileStorageService fileService, ICategoryServices categoryServices, IRepository<Picture> pictureRepository)
        {
            _entityRepository = entityRepository;
            _fileService = fileService;
            _categoryServices = categoryServices;
            _pictureRepository = pictureRepository;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Online)
                    .Include(x => x.MeasruingUnit)
                    .Include(x => x.Category)
                    .OrderBy(c => c.CreatedOnUtc);
                var entities = await query.ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        //get products for freeze when freeze a store
        public async Task<List<Product>> GetProductsByStoreIdALL(long storeId)
        {
                return await _entityRepository.TableNoTracking
                    .Where(x => x.StoreId == storeId).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdALL(long categoryId)
        {
            return await _entityRepository.TableNoTracking
                .Where(x => x.CategoryId == categoryId).ToListAsync();
        }
        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                    .Include(x => x.MeasruingUnit)
                    .Include(x => x.Category)
                    .SingleOrDefaultAsync(x => x.Id == id);

                ProductDetailsDTO productVM = new();
                productVM.Product = entity;
                productVM.Colors = JsonConvert.DeserializeObject<List<string>>(entity.Colors);
                productVM.Images = JsonConvert.DeserializeObject<List<string>>(entity.Image);
                productVM.UpperCategories = await _categoryServices.GetUpperCategories(entity.Category);

                return ApiResponse.Create(HttpStatusCode.OK, productVM);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetProductForUser(long id, Currency userCurrency)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                    .Include(x => x.MeasruingUnit)
                    .Include(x => x.Category)
                    .Include(x=>x.Currency)
                    .SingleOrDefaultAsync(x => x.Id == id);

                ProductDetailsDTO productDetailsDTO = new();
                productDetailsDTO.Product = entity;
                productDetailsDTO.Colors = JsonConvert.DeserializeObject<List<string>>(entity.Colors);
                productDetailsDTO.Images = JsonConvert.DeserializeObject<List<string>>(entity.Image);
                productDetailsDTO.UpperCategories = await _categoryServices.GetUpperCategories(entity.Category);

                ProductDetailsUserDTO productDetailsUserDTO = new();
                productDetailsUserDTO.ProductDetailsDTO = productDetailsDTO;
                productDetailsUserDTO.UserCurrency = userCurrency;
                productDetailsUserDTO.PriceUser = GetPriceInUserCurrenct(entity.Currency.Rate, userCurrency.Rate, entity.Price);
                productDetailsUserDTO.PriceUser1 = GetPriceInUserCurrenct(entity.Currency.Rate, userCurrency.Rate, entity.Price1);
                productDetailsUserDTO.PriceUser2 = GetPriceInUserCurrenct(entity.Currency.Rate, userCurrency.Rate, entity.Price2);
                productDetailsUserDTO.PriceUser3 = GetPriceInUserCurrenct(entity.Currency.Rate, userCurrency.Rate, entity.Price3);

                return ApiResponse.Create(HttpStatusCode.OK, productDetailsUserDTO);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(AddProductVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                var code = $"Product_{max + 1}";
                Product product = new()
                {
                    Code = code,
                    Image = await HelpImages(entity.Images, code),
                    Colors = HelpColors(entity.Colors),

                    CodeStore = entity.CodeStore,
                    Brand = entity.Brand,
                    Name = entity.Name,
                    Description = entity.Description,
                    MeasruingUnitId = entity.MeasruingUnitId,
                    MinValue = entity.MinValue,
                    Price = entity.Price,
                    Quantity1 = entity.Quantity1,
                    Price1 = entity.Price1,
                    Quantity2 = entity.Quantity2,
                    Price2 = entity.Price2,
                    Quantity3 = entity.Quantity3,
                    Price3 = entity.Price3,
                    CategoryId = entity.CategoryId,
                    Note = entity.Note,
                    StoreId = entity.StoreId,
                    CurrencyId = entity.CurrencyId
                };
                var newEntity = await _entityRepository.InsertAsync(product);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        private async Task<string> HelpImages(List<string> images, string code)
        {
            List<string> imagesURLs = new();
            foreach (var item in images)
            {
                Picture pic = new();
                var newPic = await _pictureRepository.InsertAsync(pic);
                var folder = newPic.Id % 10000;
                var newPath = path + $"/{folder}";
                var entityImage = Convert.FromBase64String(item);
                var imageURL = await _fileService.SaveFile(entityImage, "jpg", newPath, code);
                imagesURLs.Add(imageURL);
            }

            return JsonConvert.SerializeObject(imagesURLs);
        }

        private async Task<string> HelpEditImages(List<string> oldImages, List<string> newImages, List<string> dbImages, string code)
        {
            List<string> imagesURLs = new();
            foreach (var item in newImages)
            {
                Picture pic = new();
                var newPic = await _pictureRepository.InsertAsync(pic);
                var folder = newPic.Id % 10000;
                var newPath = path + $"/{folder}";
                var entityImage = Convert.FromBase64String(item);
                var imageURL = await _fileService.SaveFile(entityImage, "jpg", newPath, code);
                imagesURLs.Add(imageURL);
            }

            foreach (var item in dbImages)
            {
                if (oldImages.Contains(item))
                    imagesURLs.Add(item);
                else
                {
                    int pFrom = item.IndexOf(path) + path.Length + 1;
                    int pTo = item.LastIndexOf("\\");
                    string containerName = item.Substring(pFrom, pTo - pFrom);
                    await _fileService.DeleteFile(item, $"{path}/{containerName}");
                }
            }
            return JsonConvert.SerializeObject(imagesURLs);
        }

        private static string HelpColors(List<string> colors)
        {
            return JsonConvert.SerializeObject(colors);
        }

        private static string HelpEditColors(List<string> oldColors, List<string> newColors)
        {
            foreach (var item in oldColors)
                newColors.Add(item);

            return JsonConvert.SerializeObject(newColors);
        }

        public async Task<ApiResponse> Put(EditProductVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);

                var dbImages = JsonConvert.DeserializeObject<List<string>>(entityDB.Image);

                entityDB.Image = await HelpEditImages(entity.OldImages, entity.Images, dbImages, entityDB.Code);
                entityDB.Colors = HelpEditColors(entity.OldColors, entity.Colors);

                entityDB.StoreId = entity.StoreId;
                entityDB.CodeStore = entity.CodeStore;
                entityDB.Status = entity.Status;
                entityDB.CategoryId = entity.CategoryId;
                entityDB.Brand = entity.Brand;
                entityDB.Name = entity.Name;
                entityDB.Description = entity.Description;
                entityDB.MeasruingUnitId = entity.MeasruingUnitId;
                entityDB.Price = entity.Price;
                entityDB.Quantity1 = entity.Quantity1;
                entityDB.Price1 = entity.Price1;
                entityDB.Quantity2 = entity.Quantity2;
                entityDB.Price2 = entity.Price2;
                entityDB.Quantity3 = entity.Quantity3;
                entityDB.Price3 = entity.Price3;
                entityDB.CurrencyId = entity.CurrencyId;

                await _entityRepository.UpdateAsync(entityDB);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Delete(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);

                var imagesList = JsonConvert.DeserializeObject<List<string>>(entity.Image);

                foreach (var item in imagesList)
                {
                    int pFrom = item.IndexOf(path) + path.Length + 1;
                    int pTo = item.LastIndexOf("\\");
                    string containerName = item.Substring(pFrom, pTo - pFrom);
                    await _fileService.DeleteFile(item, $"{path}/{containerName}");
                }

                await _entityRepository.DeleteAsync(entity);
                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Status = entity.Status;
                await _entityRepository.UpdateAsync(entityDB);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        #region helper
        private static decimal GetPriceInUserCurrenct(double ProductCurrency, double UserCurrency, decimal Price)
        {
            decimal priceInBase = decimal.Divide(Price, Convert.ToDecimal(ProductCurrency));
            decimal priceInUserCurrency = decimal.Multiply(priceInBase, Convert.ToDecimal(UserCurrency));
            return priceInUserCurrency;
        }
        #endregion
    }
}
