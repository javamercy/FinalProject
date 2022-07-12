using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class ProductManager : IProductService

    {
        private IProductDal _productDal;
        private ICategoryService _categoryService;



        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(
                CheckByCategoryForProductCount(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryCountLimitExceeded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }



        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new Result(true, "Ürün başarıyla silindi.");
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 17)
            {

                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>
                (_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id), Messages.ProductsListedByCategoryId);
        }

        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new ErrorDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(Product))]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult("Ürün başarıyla güncellendi.");
        }


        private IResult CheckByCategoryForProductCount(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId);

            if (result.Count <= 10)
            {

                return new SuccessResult();
            }

            return new ErrorResult(Messages.ProductCountByCategoryError);
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryCountLimitExceeded()
        {
            var result = _categoryService.GetAll();

            if (result.Data.Count <= 15)
            {

                return new SuccessResult();
            }

            return new ErrorResult(Messages.CategoryCountLimitExceeded);
        }
    }
}
