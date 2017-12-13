﻿namespace LearningSystem.Web.Controllers
{
    using Data.Models;
    using Infrastructure.Extensions;
    using LearningSystem.Services.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Courses;
    using Services;
    using System.Threading.Tasks;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManager;

        public CoursesController(
            ICourseService courseService,
            UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new CourseDetailsViewModel
            {
                Course = await this.courseService.ByIdAsync<CourseDetailsServiceModel>(id)
            };

            if (model.Course == null)
            {
                return this.NotFound();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(this.User);

                model.IsUserEnrolledInCourse = await this.courseService.IsUserSignedInCourseAsync(id, userId);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUpStudent(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var success = await this.courseService.SignUpStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            this.TempData.AddSuccessMessage("You successfully enrolled in this course.");

            return this.RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOutStudent(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var success = await this.courseService.SignOutStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            this.TempData.AddSuccessMessage("You successfully signed out of this course.");

            return this.RedirectToAction(nameof(Details), new { id });
        }
    }
}
