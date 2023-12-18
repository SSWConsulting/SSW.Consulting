﻿using System.Runtime.Serialization;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;
using NUnit.Framework;
using SSW.Rewards.Application.Common.Mappings;
using SSW.Rewards.Application.Leaderboard.Queries.Common;
using SSW.Rewards.Domain.Entities;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;
using User = SSW.Rewards.Domain.Entities.User;

namespace SSW.Rewards.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(User), typeof(LeaderboardUserDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
