using System;
using AutoFixture;
using Core.Builders;
using Core.Interfaces;
using Core.Models;
using Xunit;

namespace Core.Tests
{
    public class ConnectionStringUtilityTest
    {
        private readonly IConnectionStringUtility<TestModel> _utility;
        private readonly Fixture _fixture;

        public ConnectionStringUtilityTest()
        {
            _utility = ConnectionStringUtility<TestModel>.New;
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Cycle()
        {
            // Arrange
            var guidConverter = PropertyConverterBuilderInstance.New<Guid>()
                .SetCanConvert(x => x == typeof(Guid))
                .SetParse(x => Guid.Parse(x))
                .SetToString(x => x.ToString())
                .Build();

            _utility.AddTypeConverter<Guid>(guidConverter);
            
            var instance = _fixture.Create<TestModel>();

            // Act
            var result = _utility.Parse(_utility.InstanceToString((ObjectWrapper<TestModel>) instance)).Value;

            // Assert
            Assert.Equal(instance, result, TestModelEqualityComparer.TestModelComparer);
        }
    }
}