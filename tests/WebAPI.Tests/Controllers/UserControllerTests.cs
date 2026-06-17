using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Presintation.Controllers.Users;
using Xunit;

namespace WebAPI.Tests.Controllers;

    public class UserControllerTests
    {
        private readonly IUserService _userServiceFake;
        private readonly UserController _userController;
       
       
        public UserControllerTests()
        {
            _userServiceFake = A.Fake<IUserService>();
            _userController = new UserController(_userServiceFake);
              _userController.ControllerContext = new ControllerContext{HttpContext  = new DefaultHttpContext {User = new ClaimsPrincipal(new ClaimsIdentity(new []
              {
                new Claim(ClaimTypes.Role, "SuperAdmin")
            }))}};
        }
       
        [Fact]
        public void GetAllUser_WhenUserRoleSuperAdminAndUserListFull_ShouldReturnOk()
        {
           //arrange
       
            // MethodInfo methodInfo = typeof(UserController).GetMethod("GetUserRole", BindingFlags.NonPublic | BindingFlags.Instance);
          
           var listuser = new List<User>
           {
            new User
            {
             Username = "ashraf"},new User { Username ="mohamed" }
           };
    
           A.CallTo(()=>  _userServiceFake.GetAllUserAsync()).Returns(listuser);

            //act
            var result = _userController.GetAllUsers().Result;
            //assert
            result.Should().BeOfType<OkObjectResult>();

            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(new { success = true, data  = listuser.Select(u => new Application.DTOs.Users.UserResponseDTO(

                        u.Id,u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Phone,
                        u.Role,
                        u.CreatedAt,
                        u.IsActive,
                        null,null)) });


        }
        [Fact]
        public void GetAllUser_WhenUserRoleSuperAdminAndUserListNll_ShouldReturnNotFound()
        {
           //arrange
           
           var listuser = new List<User>();
    
           A.CallTo(()=>  _userServiceFake.GetAllUserAsync()).Returns(listuser);

            //act
            var result = _userController.GetAllUsers().Result;
            //assert
            result.Should().BeOfType<NotFoundObjectResult>();

            result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(new { success = false, message = "Not have any users" });


        }
        [Fact]
        public void GetAllUser_WhenUserRoleNotSuperAdmin_ShouldReturnBadRequest()
        {
            //arrange
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Salesman")
            }));

            // MethodInfo methodInfo = typeof(UserController).GetMethod("GetUserRole", BindingFlags.NonPublic | BindingFlags.Instance);

            // A.CallTo(()=>methodInfo.Invoke(_userController, null).ToString()).Returns("Salesman");
           _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            //act
            var result = _userController.GetAllUsers().Result;
            //assert
            result.Should().BeOfType<BadRequestObjectResult>();

            result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(new { success = false, message = "Access denied" });


        }

       
    //    public async Task<IActionResult> GetUserById(int id)
    //     { 
    //             try{
    //                 var currentUserId = GetUserId();
    //                 var currentUserRole = GetUserRole();

    //                  // Users can only view their own profile unless they're SuperAdmin
    //                   if (currentUserId != id && currentUserRole != UserRole.SuperAdmin.ToString())
    //                    return HandleError("Access denied", 403);

    //                     var u = await _userService.GetUserByIdAsync(id);
                        
    //                     if(u is null )
    //                     return HandleError("User not found",404);
                        
    //                     UserResponseDTO dTO = new UserResponseDTO(
    //                         u.Id,
    //                         u.FirstName,
    //                         u.LastName,
    //                         u.Email,
    //                         u.Phone,
    //                         u.Role,
    //                         u.CreatedAt,
    //                         u.IsActive,
    //                         null,null);

    //                     return HandleResult(dTO,"User retrieved successfully");
    //         }
    //         catch(Exception ex)
    //         {
    //             return HandleError($"Failed to retrieve users: {ex.Message}");
    //         }
     [Fact]
     public void GetUserById_WhenUserIdNull_ShouldreturnHandleErrorHandleErroDeniedWith403()
    {
       //arrange
         var user = new User{};
         A.CallTo(()=> _userServiceFake.GetUserByIdAsync(1)).Returns(user);
       //act

       var result = _userController.GetUserById(0).Result;

       //assert
        result.Should().BeOfType<NotFoundObjectResult>();

          result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(new{success=false ,
           message ="User not found"});
           
    }
        
  }