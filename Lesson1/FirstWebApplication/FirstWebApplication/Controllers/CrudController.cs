using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FirstWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ValuesHolder _holder;
        public CrudController(ValuesHolder holder)
        {
            _holder = holder;
        }
        [HttpPost("create/{date}/{value}")]
        public IActionResult Create([FromRoute] string date, [FromRoute] string value)
        {
            _holder.Add(date, value); 
            return Ok();
        }
        [HttpGet("read")]
        public IActionResult Read()
        {
            return Ok(_holder.Get());
        }
        [HttpPut("update/{stringsToUpdate}/{newValue}")]
        public IActionResult Update([FromRoute] string stringsToUpdate, [FromRoute] string newValue)
        {
            for (int i = 0; i < _holder.Values.Count; i++)
            {
                if (_holder.Values[i].Date == stringsToUpdate)
                    _holder.Values[i].Value = newValue;
            }
            return Ok();
        }
        [HttpDelete("delete/{stringsToDelete}")]
        public IActionResult Delete([FromRoute] string stringsToDelete)
        {
            _holder.Values = _holder.Values.Where(w => w.Date != stringsToDelete).ToList(); 
            return Ok();
        }
    }
}
