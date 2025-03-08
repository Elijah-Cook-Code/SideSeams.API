using Microsoft.AspNetCore.Mvc;
using Data.Repositories;
using SideSeams.Data.Models; 

namespace SideSeams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            var client = _clientRepository.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _clientRepository.GetClientsAsync();
            return Ok(clients);
        }


        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientInfo client)
        {
            if (client == null)
            {
                return BadRequest("Invalid client data.");
            }

            await _clientRepository.AddClientAsync(client);
            return CreatedAtAction(nameof(GetClients), new { id = client.Id }, client);
        }



    }
}
