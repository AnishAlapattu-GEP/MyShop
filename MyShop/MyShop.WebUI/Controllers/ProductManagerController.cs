﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Interfaces;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }


        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        // GET: ProductManager/Create
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel
            {
                Product = new Product(),
                ProductCategories = productCategories.Collection()
            };

            return View(viewModel);
        }

        // POST: ProductManager/Create
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }

                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        //GET: ProductManager/Edit/1 
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel
                {
                    Product = product,
                    ProductCategories = productCategories.Collection()
                };
                return View(viewModel);
            }
        }

        //POST: ProductManager/Edit/1
        [HttpPost]
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            Product productToEdit = context.Find(id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Update(productToEdit);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        //GET: ProductManager/Delete/1
        public ActionResult Delete(string id)
        {
            Product productToDelete = context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        //POST: ProductManager/Delete/1
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToDelete = context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}