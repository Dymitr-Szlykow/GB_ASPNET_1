using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.WebAPI.Controllers
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

        public ValuesController(ILogger<ValuesController> logger) => _logger = logger;


        [HttpGet("count")]              // GET /localhost:5001/api/values/count HTTP/1.1
        public int Count() => _values.Count;


        [HttpPost]                      // POST /localhost:5001/api/values HTTP/1.1
        [HttpPost("add")]               // POST /localhost:5001/api/values/add HTTP/1.1
        [HttpPost("create")]            // POST /localhost:5001/api/values/create HTTP/1.1
        public IActionResult Create([FromBody] string newValue)
        {
            int id = _values.Count == 0 ? 1 : _values.Keys.Max() + 1;
            _values[id] = newValue;
            _logger.LogInformation("Создано значение №{id}: \"{newValue}\".", id, newValue);
            return CreatedAtAction(nameof(GetById), new { id }, newValue);
        }


        [HttpGet]                       // GET /localhost:5001/api/values HTTP/1.1
        public IActionResult GetAll()
        {
            Dictionary<int,string>.ValueCollection? values = _values.Values;
            return Ok(values);
        }


        [HttpGet("{id:int}")]           // GET /localhost:5001/api/values/42 HTTP/1.1
        public IActionResult GetById(int id)
        {
            //return _values.ContainsKey(id) ? Ok(_values[id]) : NotFound(new { id });
            return _values.TryGetValue(id, out string? value) ? Ok(value) : NotFound(new { id });
        }


        [HttpDelete("{id:int}")]        // DELETE /localhost:5001/api/values/42 HTTP/1.1
        [HttpDelete("delete/{id:int}")] // DELETE /localhost:5001/api/values/delete/42 HTTP/1.1
        public IActionResult Delete(int id)
        {
            if (!_values.ContainsKey(id))
            {
                _logger.LogWarning("Попытка удалить значение №{id}, значение не найдено.", id);
                return NotFound(new { id });
            }
            else
            {
                string? nongratum = _values[id];
                _values.Remove(id);
                _logger.LogInformation("Удалено значение №{id}: \"{nongratum}\".", id, nongratum);
                return Ok(new { Value = nongratum });
            }
        }


        [HttpPut("{id:int}")]           // PUT /localhost:5001/api/values/42 HTTP/1.1
        [HttpPut("update/{id:int}")]    // PUT /localhost:5001/api/values/update/42 HTTP/1.1
        [HttpPut("edit/{id:int}")]      // PUT /localhost:5001/api/values/edit/42 HTTP/1.1
        public IActionResult Update(int id, [FromBody] string newValue)
        {
            if (!_values.ContainsKey(id))
            {
                _logger.LogWarning("Попытка изменить значение №{id}, значение не найдено.", id);
                return NotFound(new { id });
            }
            else
            {
                string? lastValue = _values[id];
                _values[id] = newValue;
                _logger.LogInformation("Обновлено значение №{id}: \"{lastValue}\" -> \"{newValue}\".", id, lastValue, newValue);
                return Ok(new { Value = newValue });
            }
        }
    }
}