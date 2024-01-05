﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSW.Rewards.Shared.DTOs.Staff;
using SSW.Rewards.Application.Staff.Commands.DeleteStaffMemberProfile;
using SSW.Rewards.Application.Staff.Commands.UploadStaffMemberProfilePicture;
using SSW.Rewards.Application.Staff.Commands.UpsertStaffMemberProfile;
using SSW.Rewards.Application.Staff.Queries.GetStaffList;
using SSW.Rewards.Application.Staff.Queries.GetStaffMemberProfile;
using SSW.Rewards.WebAPI.Authorisation;

namespace SSW.Rewards.WebAPI.Controllers;

public class StaffController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StaffListViewModel>> Get()
    {
        return Ok(await Mediator.Send(new GetStaffListQuery()));
    }

    [HttpGet]
    public async Task<ActionResult<StaffMemberDto>> GetStaffMemberProfile(int id)
    {
        return Ok(await Mediator.Send(new GetStaffMemberProfileQuery() { Id = id }));
    }

    [HttpGet]
    public async Task<ActionResult<StaffMemberDto>> GetStaffMemberByEmail(string email)
    {
        return Ok(await Mediator.Send(new GetStaffMemberProfileQuery() { email = email, GetByEmail = true }));
    }

    [HttpPost]
    [Authorize(Roles = AuthorizationRoles.Admin)]
    public async Task<ActionResult<StaffMemberDto>> UpsertStaffMemberProfile(StaffMemberDto staffMember)
    {
        var command = new UpsertStaffMemberProfileCommand
        {
            Id = staffMember.Id,
            Name = staffMember.Name,
            Email = staffMember.Email,
            Profile = staffMember.Profile,
            TwitterUsername = staffMember.TwitterUsername,
            GitHubUsername = staffMember.GitHubUsername,
            LinkedInUrl = staffMember.LinkedInUrl,
            ProfilePhoto = staffMember.ProfilePhoto is not null ? new Uri(staffMember.ProfilePhoto) : null,
            Points = staffMember.Points,
            Skills = staffMember.Skills.ToList()
        };

        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Authorize(Roles = AuthorizationRoles.Admin)]
    public async Task<ActionResult<string>> UploadStaffMemberProfilePicture(int id, Stream file)
    {
        return Ok(await Mediator.Send(new UploadStaffMemberProfilePictureCommand { Id = id, File = file }));
    }

    [HttpDelete]
    [Authorize(Roles = AuthorizationRoles.Admin)]
    public async Task<ActionResult> DeleteStaffMemberProfile([FromQuery] int Id)
    {
        var command = new DeleteStaffMemberProfileCommand { Id = Id };
        return Ok(await Mediator.Send(command));
    }
}