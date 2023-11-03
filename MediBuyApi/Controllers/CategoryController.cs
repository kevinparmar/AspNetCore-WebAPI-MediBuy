using AutoMapper;
using MediBuyApi.CustomActionFilters;
using MediBuyApi.Models.Domain;
using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace MediBuyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categoryDTO = await categoryRepository.GetAllAsync();
            return Ok(categoryDTO);
        }


        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var categoryDTO = await categoryRepository.GetByIdAsync(Id);
            if (categoryDTO == null)
            {
                return NotFound(new { Message = "Category not found with the specified Id" });
                //throw new Exception("Category not found");
            }
            return Ok(categoryDTO);
        }

        [ValidateModel]
        // [Authorize(Roles = "Writer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EditCategoryDTO editCategoryDTO)
        {
            var categoryDomain = mapper.Map<Category>(editCategoryDTO);

            categoryDomain = await categoryRepository.Create(categoryDomain);

            var categoryDTO = mapper.Map<CategoryDTO>(categoryDomain);

            return CreatedAtAction(nameof(GetById), new { id = categoryDTO.Id }, categoryDTO);
        }

        [ValidateModel]
        [Authorize(Roles = "Writer")]
        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> Update(int Id, [FromBody] EditCategoryDTO editCategoryDTO)
        {
            var categoryDomain = mapper.Map<Category>(editCategoryDTO);

            categoryDomain = await categoryRepository.UpdateAsync(Id, categoryDomain);

            if(categoryDomain == null )
            {
                return NotFound(new { Message = "Category not found with the specified Id" });
            }
            return Ok(mapper.Map<CategoryDTO>(categoryDomain)); 
        }

        [Authorize(Roles = "Writer")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var categoryDomain = await categoryRepository.DeleteAsync(Id);
            if(categoryDomain == null )
            {
                return NotFound(new { Message = "Category not found with the specified Id" });
            }
            return Ok(mapper.Map<CategoryDTO>(categoryDomain));
        }
    }
}
