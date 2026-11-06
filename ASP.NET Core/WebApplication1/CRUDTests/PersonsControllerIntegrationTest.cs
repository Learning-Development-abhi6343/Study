// <copyright file="PersonsControllerIntegrationTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CRUDTests
{
    using Fizzler.Systems.HtmlAgilityPack;
    using FluentAssertions;
    using HtmlAgilityPack;

    /// <summary>
    /// Integration tests for the Persons controller.
    /// </summary>
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        /// <summary>
        /// The HTTP client used to send requests.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsControllerIntegrationTest"/> class.
        /// </summary>
        /// <param name="factory">The factory used to create the test client.</param>
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            this._client = factory.CreateClient();
        }

        #region Index

        /// <summary>
        /// Verifies that the Persons index endpoint returns a successful response containing a persons table.
        /// </summary>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Fact]
        public async Task Index_ToReturnView()
        {
            // Arrange

            // Act
            var response = await this._client.GetAsync("/Persons/Index", TestContext.Current.CancellationToken);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var responseBody = await response.Content.ReadAsStringAsync();

            var html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }
        #endregion
    }
}
