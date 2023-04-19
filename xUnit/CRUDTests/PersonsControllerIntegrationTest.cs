﻿using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient client;
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            client = factory.CreateClient();
        }
        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange
            HtmlDocument html = new();
            //Act
            HttpResponseMessage response = await client.GetAsync("/Persons/Index");
            var body = await response.Content.ReadAsStringAsync();
            html.LoadHtml(body);
            var document = html.DocumentNode;

            //Assert
            response.Should().BeSuccessful();
            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }
        #endregion
    }
}
