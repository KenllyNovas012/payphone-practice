using Businesslogic.DTO;
using Businesslogic.Interfaces;
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
    public class WalletsControllerTests
    {
        private Mock<IWalletRepository> _mockWalletRepository;
        private WalletsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockWalletRepository = new Mock<IWalletRepository>();
            _controller = new WalletsController(_mockWalletRepository.Object);
        }

        [TestMethod]
        public async Task GetAll()
        {
            var wallets = new List<Domain.Entities.Wallet>
        {
            new Domain.Entities.Wallet { Id = 1, DocumentId = "123", WalletName = "Wallet1", Balance = 100 },
            new Domain.Entities.Wallet { Id = 2, DocumentId = "456", WalletName = "Wallet2", Balance = 200 }
        };
            _mockWalletRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(wallets);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var dtos = okResult.Value as IEnumerable<WalletDto>;
            Assert.AreEqual(2, dtos.Count());
        }
        [TestMethod]
        public async Task GetById_ShouldReturnNotFound_WhenWalletDoesNotExist()
        {
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.Wallet)null);

            var result = await _controller.GetById(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOkResult_WhenWalletExists()
        {
            var wallet = new Domain.Entities.Wallet { Id = 1, DocumentId = "123", WalletName = "Wallet1", Balance = 100 };
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(wallet);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var dto = okResult.Value as WalletDto;
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("123", dto.DocumentId);
        }
        [TestMethod]
        public async Task Create_ShouldReturnCreatedResult_WhenWalletIsCreated()
        {
            var dto = new CreateWalletDto { DocumentId = "789", WalletName = "New Wallet", Balance = 500 };
            var wallet = new Domain.Entities.Wallet { Id = 1, DocumentId = "789", WalletName = "New Wallet", Balance = 500 };
            _mockWalletRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Wallet>())).Returns(Task.CompletedTask);
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(wallet);

            var result = await _controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetById", createdResult.ActionName);
        }
        [TestMethod]
        public async Task Update_ShouldReturnNoContent_WhenWalletIsUpdated()
        {
            var dto = new UpdateWalletDto { DocumentId = "789", WalletName = "Updated Wallet", Balance = 600 };
            var wallet = new Domain.Entities.Wallet { Id = 1, DocumentId = "123", WalletName = "Old Wallet", Balance = 500 };
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(wallet);
            _mockWalletRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Wallet>())).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, dto);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenWalletIsDeleted()
        {
            _mockWalletRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }

}
