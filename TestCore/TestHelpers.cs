using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
namespace TestCore
{
    public class TestHelpers
    {
        
        private ProductCategoryService _service;
        private Mock<IProductCategoryRepository> _mockRepo;
        private Mock<IProductRepository> _mockProductRepo;

        [SetUp]
        public void Setup()
        {
            // Mock repositories aanmaken zodat er geen echte database nodig is
            _mockRepo = new Mock<IProductCategoryRepository>();
            _mockProductRepo = new Mock<IProductRepository>();

            // De service aanmaken met de gemockte repositories
            _service = new ProductCategoryService(_mockRepo.Object, _mockProductRepo.Object);
        }


        //Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }


        //Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ";
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ")]
        public void TestPasswordHelperReturnsFalse(string password, string passwordHash)
        {
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, passwordHash));
        }
        [Test]
        public async Task GetByCategoryIdAsync_EmptyCategory_ReturnsEmptyList()
        {
            // Arrange
            int categoryId = 3;
            _mockRepo.Setup(r => r.GetProductIdsByCategoryIdAsync(categoryId))
                .ReturnsAsync(new List<int>()); // Leeg
            _mockProductRepo.Setup(p => p.GetAllAsync())
                .ReturnsAsync(new List<Product>()); // Geen producten

            // Act
            var result = await _service.GetByCategoryIdAsync(categoryId);

            // Assert
            Assert.That(result, Is.Empty);
        }
        [Test]
        public async Task GetByCategoryIdAsync_ProductRepoThrowsException_ReturnsEmptyList()
        {
            // Arrange
            int categoryId = 2;
            _mockRepo.Setup(r => r.GetProductIdsByCategoryIdAsync(categoryId))
                .ReturnsAsync(new List<int> { 1 });
            _mockProductRepo.Setup(p => p.GetAllAsync())
                .ThrowsAsync(new Exception("Navigation failed"));

            // Act
            var result = await _service.GetByCategoryIdAsync(categoryId);

            // Assert
            Assert.That(result, Is.Empty);
        }
        
    }
}