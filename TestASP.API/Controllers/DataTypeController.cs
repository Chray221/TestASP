﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.API.Extensions;
using TestASP.Model;
using TestASP.Data;
using TestASP.Domain.Contexts;
using TestASP.API.Helpers;

namespace TestASP.API.Controllers
{
    [SwaggerTag("DataTypeTables")]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataTypeController : ControllerBase
    {
        private readonly TestDbContext _context;

        public IMapper _mapper { get; }

        public DataTypeController(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DataType
        [HttpGet]
        public async Task<IActionResult> GetDataTypeTables()
        {
          if (_context.DataTypeTables == null)
          {
              return NotFound();
          }
            var list = await _context.DataTypeTables.ToListAsync();
            return MessageHelper.Ok(_mapper.Map<List<DataTypeDto>>(list), "Success");
        }

        // GET: api/DataType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataTypeTable>> GetDataTypeTable(int id)
        {
          if (_context.DataTypeTables == null)
          {
              return NotFound();
          }
            var dataTypeTable = await _context.DataTypeTables.FindAsync(id);

            if (dataTypeTable == null)
            {
                return NotFound();
            }

            return dataTypeTable;
        }

        // PUT: api/DataType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataTypeTable(int id, DataTypeTable dataTypeTable)
        {
            if (id != dataTypeTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(dataTypeTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataTypeTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DataType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DataTypeTable>> PostDataTypeTable(DataTypeTable dataTypeTable)
        {
          if (_context.DataTypeTables == null)
          {
              return Problem("Entity set 'TestDbContext.DataTypeTables'  is null.");
          }
            _context.DataTypeTables.Add(dataTypeTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDataTypeTable", new { id = dataTypeTable.Id }, dataTypeTable);
        }

        // DELETE: api/DataType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDataTypeTable(int id)
        {
            if (_context.DataTypeTables == null)
            {
                return NotFound();
            }
            var dataTypeTable = await _context.DataTypeTables.FindAsync(id);
            if (dataTypeTable == null)
            {
                return NotFound();
            }

            _context.DataTypeTables.Remove(dataTypeTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DataTypeTableExists(int id)
        {
            return (_context.DataTypeTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
