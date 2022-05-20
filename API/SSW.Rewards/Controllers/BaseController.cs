﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SSW.Rewards.WebAPI.Filters;
using SSW.Rewards.WebAPI.Security;

namespace SSW.Rewards.WebAPI.Controllers
{
    [Authorize]
    [CustomExceptionFilter]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}

