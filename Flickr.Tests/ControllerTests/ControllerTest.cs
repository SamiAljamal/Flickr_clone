using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flickr_Clone.Controllers;
using Flickr_Clone.Models;
using Xunit;

namespace Flickr.Tests.ControllerTests
{
    public class ControllerTest
    {
        [Fact]
        public void Get_ViewResult_Index_Test()
        {
            ImageController controller = new ImageController();
            var result = controller.Index();
            Assert.IsType<IActionResult>(result);
        }
    }
}
