using AutoMapper;
using MediBuyApi.Models.DTO;
using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Drawing;

namespace MediBuyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] string? nameFilter,
            [FromQuery] string? descriptionFilter,
            [FromQuery] string? sellerFilter,
            [FromQuery] int? lessThanPrice,
            [FromQuery] int? greaterThanPrice,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 1000)
        {

            var productDTO = await productRepository.GetAllAsync(nameFilter,
                                                                    descriptionFilter,
                                                                    sellerFilter,
                                                                    lessThanPrice,
                                                                    greaterThanPrice,
                                                                    sortBy,
                                                                    isAscending,
                                                                    pageNumber,
                                                                    pageSize);

            if(productDTO.Count == 0)
            {
                return NotFound(new { Message = "No products found with selected filters" });
            }

            //return Ok(mapper.Map<List<ProductDTO>>(productDomain));
            return Ok(productDTO);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var productDTO = await productRepository.GetByIdAsync(Id);

            if( productDTO == null )
            {
                return NotFound(new { Message = "Product not found with the specified Id" });
            }

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
        {
            var productDomain = mapper.Map<Product>(productDTO);

            productDomain = await productRepository.CreateAsync(productDomain);

            return Ok(mapper.Map<ProductDTO>(productDomain));
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int Id, [FromBody] EditProductDTO editProductDTO)
        {
            var productDomain = mapper.Map<Product>(editProductDTO);

            productDomain = await productRepository.UpdateAsync(Id, productDomain);
            
            if(productDomain == null )
            {
                return NotFound(new { Message = "Product not found with the specified Id" });
            }

            return Ok(mapper.Map<ProductDTO>(productDomain));
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            var productDomain = await productRepository.DeleteAsync(Id);

            if(productDomain == null )
            {
                return NotFound(new { Message = "Product not found with the specified Id" });
            }

            return Ok(mapper.Map<ProductDTO>(productDomain));
        }
    }
}
