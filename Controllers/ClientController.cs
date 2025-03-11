using Microsoft.AspNetCore.Mvc;
using Data.Repositories;
using SideSeams.Data.Models;
using SideSeams.API.DTOs;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // ✅ Get Client by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientInfoDto>> GetClientById(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null) return NotFound();

            var clientDto = new ClientInfoDto
            {
                Id = client.Id,
                Name = client.Name,
                Date = client.Date,
                Notes = client.Notes,
                PhoneNumber = client.PhoneNumber,
                Measurements = client.Measurements.Select(m => new MeasurementsDto
                {
                    Id = m.Id,
                    ClientId = m.ClientId,
                    A_ChestMeasurement = (double?)m.A_ChestMeasurement, // Convert int? → double?
                    B_SeatMeasurement = (double?)m.B_SeatMeasurement,
                    C_WaistMeasurement = (double?)m.C_WaistMeasurement,
                    D_TrouserMeasurement = (double?)m.D_TrouserMeasurement,
                    E_F_HalfBackMeasurement = (double?)m.E_F_HalfBackMeasurement,
                    G_H_BackNeckToWaistMeasurement = (double?)m.G_H_BackNeckToWaistMeasurement,
                    G_I_SyceDepthMeasurement = (double?)m.G_I_SyceDepthMeasurement,
                    I_L_SleeveLengthOnePieceMeasurement = (double?)m.I_L_SleeveLengthOnePieceMeasurement,
                    E_I_SleeveLengthTwoPieceMeasurement = (double?)m.E_I_SleeveLengthTwoPieceMeasurement,
                    N_InsideLegMeasurement = (double?)m.N_InsideLegMeasurement,
                    P_Q_BodyRiseMeasurement = (double?)m.P_Q_BodyRiseMeasurement,
                    R_CloseWristMeasurement = (double?)m.R_CloseWristMeasurement
                }).ToList()
            };

            return Ok(clientDto);
        }

        // ✅ Get all Clients
        [HttpGet]
        public async Task<ActionResult<List<ClientInfoDto>>> GetClients()
        {
            var clients = await _clientRepository.GetClientsAsync();

            var clientDtos = clients.Select(client => new ClientInfoDto
            {
                Id = client.Id,
                Name = client.Name,
                Date = client.Date,
                Notes = client.Notes,
                PhoneNumber = client.PhoneNumber,
                Measurements = client.Measurements.Select(m => new MeasurementsDto
                {
                    Id = m.Id,
                    ClientId = m.ClientId,
                    A_ChestMeasurement = m.A_ChestMeasurement,
                    B_SeatMeasurement = m.B_SeatMeasurement,
                    C_WaistMeasurement = m.C_WaistMeasurement,
                    D_TrouserMeasurement = m.D_TrouserMeasurement,
                    E_F_HalfBackMeasurement = m.E_F_HalfBackMeasurement,
                    G_H_BackNeckToWaistMeasurement = m.G_H_BackNeckToWaistMeasurement,
                    G_I_SyceDepthMeasurement = m.G_I_SyceDepthMeasurement,
                    I_L_SleeveLengthOnePieceMeasurement = m.I_L_SleeveLengthOnePieceMeasurement,
                    E_I_SleeveLengthTwoPieceMeasurement = m.E_I_SleeveLengthTwoPieceMeasurement,
                    N_InsideLegMeasurement = m.N_InsideLegMeasurement,
                    P_Q_BodyRiseMeasurement = m.P_Q_BodyRiseMeasurement,
                    R_CloseWristMeasurement = m.R_CloseWristMeasurement
                }).ToList()
            }).ToList();

            return Ok(clientDtos);
        }

        // ✅ Add Client
        [HttpPost]
        public async Task<ActionResult<ClientInfoDto>> AddClient(ClientInfoDto clientDto)
        {
            var client = new ClientInfo
            {
                Name = clientDto.Name,
                Date = clientDto.Date,
                Notes = clientDto.Notes,
                PhoneNumber = clientDto.PhoneNumber,
                Measurements = clientDto.Measurements.Select(m => new ClientMeasurements
                {
                    ClientId = clientDto.Id,
                    A_ChestMeasurement = (int?)m.A_ChestMeasurement, // Convert double? → int?
                    B_SeatMeasurement = (int?)m.B_SeatMeasurement,
                    C_WaistMeasurement = (int?)m.C_WaistMeasurement,
                    D_TrouserMeasurement = (int?)m.D_TrouserMeasurement,
                    E_F_HalfBackMeasurement = (int?)m.E_F_HalfBackMeasurement,
                    G_H_BackNeckToWaistMeasurement = (int?)m.G_H_BackNeckToWaistMeasurement,
                    G_I_SyceDepthMeasurement = (int?)m.G_I_SyceDepthMeasurement,
                    I_L_SleeveLengthOnePieceMeasurement = (int?)m.I_L_SleeveLengthOnePieceMeasurement,
                    E_I_SleeveLengthTwoPieceMeasurement = (int?)m.E_I_SleeveLengthTwoPieceMeasurement,
                    N_InsideLegMeasurement = (int?)m.N_InsideLegMeasurement,
                    P_Q_BodyRiseMeasurement = (int?)m.P_Q_BodyRiseMeasurement,
                    R_CloseWristMeasurement = (int?)m.R_CloseWristMeasurement
                }).ToList()
            };

            await _clientRepository.AddClientAsync(client); // ✅ No need to call SaveChangesAsync

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, clientDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientInfoDto clientDto)
        {
            var existingClient = await _clientRepository.GetClientByIdAsync(id);
            if (existingClient == null) return NotFound();

            // Update client properties
            existingClient.Name = clientDto.Name;
            existingClient.Date = clientDto.Date;
            existingClient.Notes = clientDto.Notes;
            existingClient.PhoneNumber = clientDto.PhoneNumber;

            // Update Measurements
            var existingMeasurements = existingClient.Measurements.ToList();

            foreach (var measurementDto in clientDto.Measurements)
            {
                var existingMeasurement = existingMeasurements.FirstOrDefault(m => m.Id == measurementDto.Id);

                if (existingMeasurement != null)
                {
                    // Update existing measurement
                    existingMeasurement.A_ChestMeasurement = (int?)measurementDto.A_ChestMeasurement;
                    existingMeasurement.B_SeatMeasurement = (int?)measurementDto.B_SeatMeasurement;
                    existingMeasurement.C_WaistMeasurement = (int?)measurementDto.C_WaistMeasurement;
                    existingMeasurement.D_TrouserMeasurement = (int?)measurementDto.D_TrouserMeasurement;
                    existingMeasurement.E_F_HalfBackMeasurement = (int?)measurementDto.E_F_HalfBackMeasurement;
                    existingMeasurement.G_H_BackNeckToWaistMeasurement = (int?)measurementDto.G_H_BackNeckToWaistMeasurement;
                    existingMeasurement.G_I_SyceDepthMeasurement = (int?)measurementDto.G_I_SyceDepthMeasurement;
                    existingMeasurement.I_L_SleeveLengthOnePieceMeasurement = (int?)measurementDto.I_L_SleeveLengthOnePieceMeasurement;
                    existingMeasurement.E_I_SleeveLengthTwoPieceMeasurement = (int?)measurementDto.E_I_SleeveLengthTwoPieceMeasurement;
                    existingMeasurement.N_InsideLegMeasurement = (int?)measurementDto.N_InsideLegMeasurement;
                    existingMeasurement.P_Q_BodyRiseMeasurement = (int?)measurementDto.P_Q_BodyRiseMeasurement;
                    existingMeasurement.R_CloseWristMeasurement = (int?)measurementDto.R_CloseWristMeasurement;
                }
                else
                {
                    // Add new measurement if it doesn't exist
                    existingClient.Measurements.Add(new ClientMeasurements
                    {
                        ClientId = id,
                        A_ChestMeasurement = (int?)measurementDto.A_ChestMeasurement,
                        B_SeatMeasurement = (int?)measurementDto.B_SeatMeasurement,
                        C_WaistMeasurement = (int?)measurementDto.C_WaistMeasurement,
                        D_TrouserMeasurement = (int?)measurementDto.D_TrouserMeasurement,
                        E_F_HalfBackMeasurement = (int?)measurementDto.E_F_HalfBackMeasurement,
                        G_H_BackNeckToWaistMeasurement = (int?)measurementDto.G_H_BackNeckToWaistMeasurement,
                        G_I_SyceDepthMeasurement = (int?)measurementDto.G_I_SyceDepthMeasurement,
                        I_L_SleeveLengthOnePieceMeasurement = (int?)measurementDto.I_L_SleeveLengthOnePieceMeasurement,
                        E_I_SleeveLengthTwoPieceMeasurement = (int?)measurementDto.E_I_SleeveLengthTwoPieceMeasurement,
                        N_InsideLegMeasurement = (int?)measurementDto.N_InsideLegMeasurement,
                        P_Q_BodyRiseMeasurement = (int?)measurementDto.P_Q_BodyRiseMeasurement,
                        R_CloseWristMeasurement = (int?)measurementDto.R_CloseWristMeasurement
                    });
                }
            }

            await _clientRepository.UpdateClientAsync(existingClient);
            return NoContent();
        }


        // ✅ Delete Client
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null) return NotFound();

            await _clientRepository.DeleteClientAsync(id); // ✅ Automatically saves changes
            return NoContent();
        }
    }
}
