using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Client.Helpers;
using AD.Server.Data;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AD.Shared.Entities.MTM;

namespace AD.Server.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Category> _entityRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CategoryFollowers> _categoryFollowersRepository;
        private readonly IUserCServices _userCServices;
        private readonly IFileStorageService _fileService;
        private const string path = "categories";
        public CategoryServices(IRepository<Category> entityRepository, IFileStorageService fileService, IRepository<Product> productRepository, ApplicationDbContext context, IRepository<CategoryFollowers> categoryFollowersRepository, IUserCServices userCServices)
        {
            _entityRepository = entityRepository;
            _fileService = fileService;
            _productRepository = productRepository;
            _context = context;
            _categoryFollowersRepository = categoryFollowersRepository;
            _userCServices = userCServices;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).Include(x => x.Parent);
                var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetCategoriesWithoutProducts()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).Include(x => x.Parent).OrderByDescending(x => x.Id);
                var list = await query.ToListAsync();
                foreach (var item in query)
                {
                    bool categoryHasProducts = await IsCategoryHasProducts(item.Id);
                    if (categoryHasProducts)
                        list.RemoveAll(x => x.Id == item.Id);
                }
                return ApiResponse.Create(HttpStatusCode.OK, list);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                    .Include(x => x.Parent)
                    .Include(x=>x.CategoryFollowers).SingleOrDefaultAsync(x => x.Id == id);
                CategoryDTO categoryDTO = new()
                {
                    Category = entity,
                    UpperCategories = await GetUpperCategories(entity),
                    ChildCategories = await GetChildCategories(entity)
                };
                return ApiResponse.Create(HttpStatusCode.OK, categoryDTO);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetDetailsForUser(long id, string userName)
        {
            try
            {
                CategoryDetailsForUser categoryDetailsForUser = new();

                var entity = await _entityRepository.TableNoTracking
                    .Include(x => x.Parent)
                    .Include(x => x.CategoryFollowers).SingleOrDefaultAsync(x => x.Id == id);

                var user = _userCServices.GetCurrent(userName);
                var userId = user != null ? user.Id : 0;
                var categoryFollower = await _categoryFollowersRepository.TableNoTracking
                    .Where(x => x.CategoryId == id && x.UserCId == userId).SingleOrDefaultAsync();
                FollowStatus followStatus = categoryFollower != null ? categoryFollower.Status : FollowStatus.UnFollow;        
                categoryDetailsForUser.Category = new CategoryForUser() { Category = entity, Status = followStatus };

                var childCategories = await GetChildCategories(entity);
                foreach (var item in childCategories)
                {
                    var childCategoryFollower = await _categoryFollowersRepository.TableNoTracking
                     .Where(x => x.CategoryId == item.Id && x.UserCId == userId).SingleOrDefaultAsync();
                    FollowStatus childFollowStatus = childCategoryFollower != null ? childCategoryFollower.Status : FollowStatus.UnFollow;
                    categoryDetailsForUser.ChildCategories.Add(new CategoryForUser() { Category = item, Status = childFollowStatus });
                }
                return ApiResponse.Create(HttpStatusCode.OK, categoryDetailsForUser);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<List<Category>> GetUpperCategories(Category category)
        {
            List<Category> result = new();
            var list = await _entityRepository.TableNoTracking.Include(x => x.Parent).ToListAsync();
            while (category.ParentId != null)
            {
                Category c = list.Where(x => x.Id == category.ParentId).SingleOrDefault();
                result.Add(c);
                category.ParentId = c.ParentId;
            }
            return result;
        }
        
        public async Task<List<Category>> GetChildCategories(Category category)
        {
            var list = await _entityRepository.TableNoTracking
                .Where(x => x.ParentId == category.Id)
                .Include(x => x.Parent).ToListAsync();
            return list;
        }

        public async Task<ApiResponse> Post(AddCategoryVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                Category category = new()
                {
                    Name_ar = entity.Name_ar,
                    Name_en = entity.Name_en,
                    Name_fr = entity.Name_fr,
                    Name_tr = entity.Name_tr,
                    Name_ru = entity.Name_ru,
                    Description_ar = entity.Description_ar,
                    Description_en = entity.Description_en,
                    Description_fr = entity.Description_fr,
                    Description_tr = entity.Description_tr,
                    Description_ru = entity.Description_ru,
                    Image = entity.Image,
                    CategoryLevel = entity.CategoryLevel,
                    ParentId = entity.ParentId,
                    Code = $"Category_{max + 1}"
                };
                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    var entityImage = Convert.FromBase64String(entity.Image);
                    category.Image = await _fileService.SaveFile(entityImage, "jpg", path, category.Code);
                }
                var newEntity = await _entityRepository.InsertAsync(category);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EditCategoryVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);

                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    if (entityDB.Image != null)
                        await _fileService.DeleteFile(entityDB.Image, path);

                    var entityImage = Convert.FromBase64String(entity.Image);
                    entityDB.Image = await _fileService.EditFile(entityImage, "jpg", path, entityDB.Image, entityDB.Code);
                }
                entityDB.Name_ar = entity.Name_ar;
                entityDB.Name_en = entity.Name_en;
                entityDB.Name_fr = entity.Name_fr;
                entityDB.Name_ru = entity.Name_ru;
                entityDB.Name_tr = entity.Name_tr;
                entityDB.Description_ar = entity.Description_ar;
                entityDB.Description_en = entity.Description_en;
                entityDB.Description_fr = entity.Description_fr;
                entityDB.Description_ru = entity.Description_ru;
                entityDB.Description_tr = entity.Description_tr;
                entityDB.Status = entity.Status;
                await _entityRepository.UpdateAsync(entityDB);

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

                await FreezeWithChildren(entityDB, entity.Status);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        #region Helper
        public async Task FreezeWithChildren(Category parent, Status status)
        {
            parent.Status = status;
            await _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE Categories SET Status = {(int)status} WHERE Id = {parent.Id};");

            var children = await GetChildCategories(parent);
            if (children.Any())
            {
                foreach (var item in children)
                {
                    parent.Status = status;
                    await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE Categories SET Status = {(int)status} WHERE Id = {parent.Id};");

                    await FreezeWithChildren(item, status);
                }
            }
            else
            {
                var categoryProducts = await _productRepository.TableNoTracking.Where(x => x.CategoryId == parent.Id).ToListAsync();
                foreach (var item in categoryProducts)
                {
                    var entityDB = await _productRepository.GetByIdAsync(item.Id);
                    entityDB.Status = status;
                    await _productRepository.UpdateAsync(entityDB);
                }
            }
        }
        #endregion

        public async Task<ApiResponse> Delete(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);

                if (!string.IsNullOrWhiteSpace(entity.Image))
                    await _fileService.DeleteFile(entity.Image, path);

                await _entityRepository.DeleteAsync(entity);
                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<bool> IsCategoryHasProducts(long categoryId)
        {
            var result = await _productRepository.AnyAsync(x => x.CategoryId == categoryId);
            return result;
        }
    }
}