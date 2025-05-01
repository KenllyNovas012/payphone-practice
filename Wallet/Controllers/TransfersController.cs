using Businesslogic.DTO;
using Businesslogic.Interfaces;
using Businesslogic.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferRepository _txRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly TransferService _transferService;

        public TransfersController(ITransferRepository txRepo, IWalletRepository walletRepo)
        {
            _txRepo = txRepo;
            _walletRepo = walletRepo;
            _transferService = new TransferService(walletRepo, txRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var transfers = await _txRepo.GetAllAsync();
                var dtos = transfers.Select(t => new TransferDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount,
                    TransferType = t.TransferType,
                    CreatedAt = t.CreatedAt,
                    Wallet = new WalletDto
                    {
                        Id = t.Wallet.Id,
                        DocumentId = t.Wallet.DocumentId,
                        WalletName = t.Wallet.WalletName,
                        Balance = t.Wallet.Balance
                    }
                });
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener transferencias: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto request)
        {
            try
            {
                await _transferService.TransferAsync(request.FromWalletId, request.ToWalletId, request.Amount);
                return Ok("Transferencia realizada exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado al realizar la transferencia: {ex.Message}");
            }
        }
    }
}
