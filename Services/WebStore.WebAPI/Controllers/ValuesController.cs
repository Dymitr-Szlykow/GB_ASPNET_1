using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASP.NET.WebStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<int, string> _values
            = Enumerable
                .Range(1, 10)
                .Select(el => (Id: el, Value: $"value-{el}"))
                .ToDictionary(el => el.Id, el => el.Value);

        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }


        [HttpGet("count")]              // GET /localhost:5001/api/values/count HTTP/1.1
        public int Count() => _values.Count;


        [HttpPost]                      // POST /localhost:5001/api/values HTTP/1.1
        [HttpPost("add")]               // POST /localhost:5001/api/values/add HTTP/1.1
        [HttpPost("create")]            // POST /localhost:5001/api/values/create HTTP/1.1
        public IActionResult Create([FromBody] string newValue)
        {
            var id = _values.Count == 0 ? 1 : _values.Keys.Max() + 1;
            _values[id] = newValue;
            return CreatedAtAction(nameof(GetById), new { id }, newValue);
        }


        [HttpGet]                       // GET /localhost:5001/api/values HTTP/1.1
        public IEnumerable<string> GetAll() => _values.Values;


        [HttpDelete("{id:int}")]        // DELETE /localhost:5001/api/values/42 HTTP/1.1
        [HttpDelete("delete/{id:int}")] // DELETE /localhost:5001/api/values/delete/42 HTTP/1.1
        public IActionResult Delete(int id)
        {
            if (!_values.ContainsKey(id)) return NotFound();

            _values.Remove(id);
            return Ok();
        }


        [HttpGet("{id:int}")]           // GET /localhost:5001/api/values/42 HTTP/1.1
        public IActionResult GetById(int id)
        {
            //return _values.ContainsKey(id)
            //    ? Ok(_values[id])     // Ok(new { Id = id })
            //    : NotFound();
            return _values.TryGetValue(id, out var value) ? Ok(value) : NotFound();
        }


        [HttpPut("{id:int}")]          // PUT /localhost:5001/api/values/42 HTTP/1.1
        [HttpPut("update/{id:int}")]   // PUT /localhost:5001/api/values/update/42 HTTP/1.1
        [HttpPut("edit/{id:int}")]     // PUT /localhost:5001/api/values/edit/42 HTTP/1.1
        public IActionResult Update(int id, [FromBody] string newValue)
        {
            if (!_values.ContainsKey(id)) return NotFound();

            _values[id] = newValue;
            return Ok();
        }
    }
}