﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context =context;
        }

        // GET api/values
        [HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //public IActionResult  GetValues()
        public async Task<IActionResult>  GetValues()
        {
            
            //return new string[] { "value1", "value2", "value3", "value4" };

            //var values = _context.Values.ToList();

            var values = await _context.Values.ToListAsync();

            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        public IActionResult GetValue(int id)
        //public async Task<IActionResult> GetValue(int id)
        {
            //return "value";

            var value = _context.Values.FirstOrDefault(x => x.id == id);
            //var value = await _context.Values.FirstOrDefaultAsync(x => x.id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}