﻿using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly Logger<CatalogController> _logger;

    public CatalogController(IProductRepository productRepository)
    {
        this._productRepository = productRepository;
    }

    [HttpGet]
    public string Login(string id)
    {
        return $"Login {id}";
    }

    [HttpGet]
    public string GetName()
    {
        return "My NAME IS Success!";
    }

    [HttpGet]
    public string LoginOut(string id)
    {
        return $"Login {id}";
    }

    [HttpGet]
    //[ProducesResponseType(typeof(IList<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<Product>>> GetProduct()
    {
        var products = await _productRepository.GetProducts();
        return Ok(products.ToList());
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _productRepository.GetProduct(id);
        if (product == null)
        {
            _logger.LogError($"Product with id: {id}, not found!");
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var products = await _productRepository.GetProductByCategory(category);
        return Ok(products);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
    {
        var items = await _productRepository.GetProductByName(name);
        if (items == null)
        {
            _logger.LogError($"Products with name: {name} not found.");
            return NotFound();
        }
        return Ok(items);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await _productRepository.CreateProduct(product);

        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        return Ok(await _productRepository.UpdateProduct(product));
    }

    [HttpGet]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteProductById(string id)
    {
        return Ok(await _productRepository.DeleteProduct(id));
    }



}
