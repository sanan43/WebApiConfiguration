﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TaskWebAPI.DAL.EFCore;
using TaskWebAPI.Entities;
using TaskWebAPI.Entities.Dtos.Cars;

namespace TaskWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CarsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Cars.ToListAsync();
            if (result==null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK,result);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Cars.Where(b=>b.Id==id).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCarDto carDto)
        {
            //Car car = new Car()
            //{
            //    BrandId = carDto.BrandId,
            //    ColorId = carDto.ColorId,
            //    ModelYear = carDto.ModelYear,
            //    DailyPrice = carDto.DailyPrice,
            //    Description = carDto.Description,
            //};
            Car car = _mapper.Map<Car>(carDto);
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id,UpdateCarDto updateCarDto)
        {
            
            Car result = await _context.Cars.FindAsync(id);
            
            if (result == null) { return  NotFound(); }
            result.Description = updateCarDto.Description;
            result.DailyPrice = updateCarDto.DailyPrice;
            result.BrandId = updateCarDto.BrandId;
            result.ColorId = updateCarDto.ColorId;
            result.ModelYear = updateCarDto.ModelYear;
            //Car car = _mapper.Map<Car>(updateCarDto);
            // _context.Cars.Update(car);

            await _context.SaveChangesAsync();
                return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Cars.FindAsync(id);
            if (result == null) return BadRequest(new
            {
                StatusCode = 201,
                Message = "Tapilmadi"
            });
            _context.Cars.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
            
        }

    }
}
