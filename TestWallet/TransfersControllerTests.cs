using Businesslogic.DTO;
using Businesslogic.Interfaces;
using Businesslogic.UseCases;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Api.Controllers;

namespace TestWallet
{
    [TestClass]
    public class  TransfersControllerTests
    {
        private Mock<ITransferRepository> _transferRepoMock = null!;
        private Mock<IWalletRepository> _walletRepoMock = null!;
        private Mock<TransferService> _transferServiceMock = null!;
        private TransfersController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _transferRepoMock = new Mock<ITransferRepository>();
            _walletRepoMock = new Mock<IWalletRepository>();
            _transferServiceMock = new Mock<TransferService>(_walletRepoMock.Object, _transferRepoMock.Object);
            _controller = new TransfersController(_transferRepoMock.Object, _walletRepoMock.Object);
        }

        [TestMethod]
        public async Task Transfer_ReturnsOkResult_WhenTransferIsSuccessful()
        {
            var transferRequest = new TransferRequestDto
            {
                FromWalletId = 123,
                ToWalletId = 456,
                Amount = 100.00m
            };

            _transferServiceMock
                .Setup(service => service.TransferAsync(transferRequest.FromWalletId, transferRequest.ToWalletId, transferRequest.Amount))
                .Returns(Task.CompletedTask); 

            var result = await _controller.Transfer(transferRequest);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Transferencia realizada exitosamente.", okResult.Value);
        }

        [TestMethod]
        public async Task Transfer_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
        {
            var transferRequest = new TransferRequestDto
            {
                FromWalletId = 123,
                ToWalletId = 456,
                Amount = 100.00m
            };

            _transferServiceMock
                .Setup(service => service.TransferAsync(transferRequest.FromWalletId, transferRequest.ToWalletId, transferRequest.Amount))
                .Throws(new ArgumentException("Argumento inválido"));

            var result = await _controller.Transfer(transferRequest);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Argumento inválido", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Transfer_ReturnsConflict_WhenInvalidOperationExceptionIsThrown()
        {
            var transferRequest = new TransferRequestDto
            {
                FromWalletId = 123,
                ToWalletId = 456,
                Amount = 100.00m
            };

            _transferServiceMock
                .Setup(service => service.TransferAsync(transferRequest.FromWalletId, transferRequest.ToWalletId, transferRequest.Amount))
                .Throws(new InvalidOperationException("Operación no permitida"));

            var result = await _controller.Transfer(transferRequest);

            var conflictResult = result as StatusCodeResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);
        }

        [TestMethod]
        public async Task Transfer_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            var transferRequest = new TransferRequestDto
            {
                FromWalletId = 123,
                ToWalletId = 456,
                Amount = 100.00m
            };

            _transferServiceMock
                .Setup(service => service.TransferAsync(transferRequest.FromWalletId, transferRequest.ToWalletId, transferRequest.Amount))
                .Throws(new Exception("Error inesperado"));

            var result = await _controller.Transfer(transferRequest);

            var internalServerErrorResult = result as StatusCodeResult;
            Assert.IsNotNull(internalServerErrorResult);
            Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        }
    }

}
