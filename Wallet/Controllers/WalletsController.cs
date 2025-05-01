using Businesslogic.DTO;
using Domain.Entities;
using Businesslogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;

        public WalletsController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var wallets = await _walletRepository.GetAllAsync();
                var dtos = wallets.Select(w => new WalletDto
                {
                    Id = w.Id,
                    DocumentId = w.DocumentId,
                    WalletName = w.WalletName,
                    Balance = w.Balance
                });
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener billeteras: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(id);
                if (wallet == null) return NotFound();

                var dto = new WalletDto
                {
                    Id = wallet.Id,
                    DocumentId = wallet.DocumentId,
                    WalletName = wallet.WalletName,
                    Balance = wallet.Balance
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener billetera: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetByDocumentId")]
        public async Task<IActionResult> GetByDocumentId(string documentId)
        {
            try
            {
                var wallet = await _walletRepository.GetByDocumentIdAsync(documentId);
                if (wallet == null) return NotFound();

                var dto = new WalletDto
                {
                    Id = wallet.Id,
                    DocumentId = wallet.DocumentId,
                    WalletName = wallet.WalletName,
                    Balance = wallet.Balance
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener billetera: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
        {
            try
            {
                var wallet = new Domain.Entities.Wallet
                {
                    DocumentId = dto.DocumentId,
                    WalletName = dto.WalletName,
                    Balance = dto.Balance,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _walletRepository.AddAsync(wallet);
                return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear billetera: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto dto)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(id);
                if (wallet == null) return NotFound();

                wallet.DocumentId = dto.DocumentId;
                wallet.WalletName = dto.WalletName;
                wallet.Balance = dto.Balance;
                wallet.UpdatedAt = DateTime.UtcNow;
                await _walletRepository.UpdateAsync(wallet);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar billetera: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _walletRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar billetera: {ex.Message}");
            }
        }
    }
}
