using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Models;
using Events_TenantUserApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Events_TenantUserApp.Tests.ControllerTests
{
    [TestClass]
    public class EventsControllerTests
    {
        private readonly EventsController _eventsController;

        public EventsControllerTests(IStringLocalizer<BaseController> baseLocalizer, ILogger<EventsController> logger, IConfiguration configuration)
        {
            var mockUtilities = new Mock<IUtilities>();
            var mockTenantRepo = new Mock<ITenantRepository>();
            mockTenantRepo.Setup(repo => repo.GetVenue()).Returns(GetVenue());
            mockTenantRepo.Setup(repo => repo.GetVenueType("Classic", 12345)).Returns(GetVenueType());
            mockTenantRepo.Setup(repo => repo.GetAllCountries(12345)).Returns(GetCountries());
            mockTenantRepo.Setup(repo => repo.GetEventsForTenant(12345)).Returns(GetEvents());

            _eventsController = new EventsController(mockTenantRepo.Object, baseLocalizer, logger, configuration);
        }

        [Fact]
        public void Index_ReturnsView()
        {
            // Act
            var result = _eventsController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EventModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

        }

        private async Task<VenueModel> GetVenue()
        {
            return new VenueModel
            {
                VenueName = "Venue 1",
                PostalCode = "741",
                CountryCode = "USA",
                VenueType = "Classic",
                VenueId = 1976168774
            };
        }

        private async Task<VenueTypeModel> GetVenueType()
        {
            return new VenueTypeModel
            {
                VenueType = "Classic",
                EventTypeName = "Classical Concert",
                Language = "en-us",
                EventTypeShortName = "Concert",
                EventTypeShortNamePlural = "Concerts",
                VenueTypeName = "Classical Music Venue"
            };
        }

        private async Task<List<CountryModel>> GetCountries()
        {
            return new List<CountryModel>
            {
                new CountryModel
                {
                    CountryCode = "USA",
                    CountryName = "United States",
                    Language = "en-US"
                },
                new CountryModel
                {
                    CountryCode = "MUR",
                    CountryName = "Mauritius",
                    Language = "en-UK"
                }
            };
        }

        private async Task<List<EventModel>> GetEvents()
        {
            return new List<EventModel>
            {
                new EventModel
                {
                    Date = DateTime.Now,
                    EventId = 1,
                    EventName = "String Serenades",
                    SubTitle = "Contoso Chamber Orchestra"
                },
                new EventModel
                {
                    Date = DateTime.Now,
                    EventId = 2,
                    EventName = "Concert Pops",
                    SubTitle = "Contoso Symphony"
                }
            };
        }
    }
}
